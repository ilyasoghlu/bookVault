
## Getting Started



.

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

## Installed Libraries 
    
    - Shadcn ui 
        - button
        - dropdown menu        

    - lucide-react (React Icons)

## HttpOnly cookie əsaslı authentication

    Auth arxitekturasını müəyyənləşdirək
    Login API → cookie mexanizmini quraq
    Frontend-də auth state yaradaq
    GET /me / current-user yoxlamasını quraq
    Protected routes yaradaq
    Logout quraq
    Access token + refresh token strategiyasını quraq
    Token expiration və avtomatik refresh
    Loading/error states
    Production security: CORS, CSRF, Secure cookies və s.