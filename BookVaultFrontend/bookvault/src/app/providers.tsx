'use client'

import { ThemeProvider} from "./theme-provider"
import { AuthProvider } from "@/components/auth/AuthProvider"

function Providers({children}: {children: React.ReactNode }){
    return (
        <AuthProvider>
            <ThemeProvider 
                attribute='class'
                defaultTheme="system"
                enableSystem
                disableTransitionOnChange
                >
                {children}
            </ThemeProvider>
        </AuthProvider>
    )
}

export default Providers