
"use client";

import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";

import {
  getMe,
  login as loginRequest,
} from "@/lib/auth/authService";

export type User = {
  id: string;
  name: string;
  email: string;
  permissions: string[];
};

type AuthContextType = {
  user: User | null;
  loading: boolean;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  hasPermission: (permission: string) => boolean;
};

const AuthContext = createContext<AuthContextType | undefined>(
  undefined
);

function decodeToken(token: string): User | null {
  try {
    const payload = token.split(".")[1];

    if (!payload) {
      return null;
    }

    const decoded = JSON.parse(
      atob(
        payload
          .replace(/-/g, "+")
          .replace(/_/g, "/")
      )
    );

    return {
      id:
        decoded.sub ??
        decoded[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ],
      name:
        decoded.name ??
        decoded[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
        ],
      email: decoded.email,
      permissions: [],
    };
  } catch {
    return null;
  }
}

export function AuthProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadUser = async () => {
      const token = localStorage.getItem("accessToken");

      if (!token) {
        setLoading(false);
        return;
      }

      const decodedUser = decodeToken(token);

      if (!decodedUser) {
        localStorage.removeItem("accessToken");
        setLoading(false);
        return;
      }

      try {
        const me = await getMe();

        setUser({
          id: me.id,
          name: me.username,
          email: decodedUser.email,
          permissions: me.permissions,
        });
      } catch {
        localStorage.removeItem("accessToken");
        setUser(null);
      } finally {
        setLoading(false);
      }
    };

    loadUser();
  }, []);

  const login = useCallback(
    async (email: string, password: string) => {
      const response = await loginRequest({
        email,
        password,
      });

      localStorage.setItem(
        "accessToken",
        response.token
      );

      const decodedUser = decodeToken(response.token);

      if (!decodedUser) {
        localStorage.removeItem("accessToken");
        throw new Error(
          "Invalid authentication token."
        );
      }

      const me = await getMe();

      setUser({
        id: me.id,
        name: me.username,
        email: decodedUser.email,
        permissions: me.permissions,
      });
    },
    []
  );

  const logout = useCallback(() => {
    localStorage.removeItem("accessToken");
    setUser(null);
  }, []);

  const hasPermission = useCallback(
    (permission: string) => {
      return (
        user?.permissions.includes(permission) ?? false
      );
    },
    [user]
  );

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        isAuthenticated: !!user,
        login,
        logout,
        hasPermission,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuthContext() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error(
      "useAuthContext must be used inside AuthProvider"
    );
  }

  return context;
}

