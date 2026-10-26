"use client"

import Image from "next/image";
import Logo from '../../../public/images/logo.png'
import { 
          Sidebar, 
          SidebarProvider, 
          SidebarContent, 
          SidebarFooter, 
          SidebarHeader, 
          SidebarMenuItem, 
          SidebarMenu
        } from '../ui/sidebar';

import Link from 'next/link';
import { sidebarLinks } from '@/utils/links';
import { Separator } from '../ui/separator';
import { Button } from '../ui/button';
import { useAuth } from '@/hooks/useAuth';
// import { useState } from "react";


function AppSidebar() {
  const {hasPermission} = useAuth();
  // const [isActive, setIsActive] = useState("Dashboard");

  const visibleLinks = sidebarLinks.filter(
    (link) =>
      !("permission" in link) || hasPermission(link.permission) 
  )

  return (
      <SidebarProvider>
            <Sidebar>
              <SidebarHeader>                
                  <header className="flex  flex-col justify-between sm:flex-row items-start max-w-6xl  mx-auto px-4 py-10 sm:px-8 " >
                    <Image src={Logo} alt="logo" className="w-6 h-6 cursor-pointer" /> 
                    <h1 className='font-semibold  uppercase '>BookVault</h1>
                </header>
              </SidebarHeader>
              <Separator />
              <div className='my-4'>
              <SidebarContent className='my-4'>
                <SidebarMenu>
                  {visibleLinks.map((link) =>(
                    <SidebarMenuItem key={link.href} className='flex flex-col items-center gap-6'> 

                        <Button 
                          className="
                              w-55 
                              h-15 
                              m-0.5
                              p-3 
                              bg-gray-200 
                              border-2 
                              border-blue-50 
                              shadow-sm 
                              hover:bg-sky-300 
                              hover:text-white">
                          <Link 
                            href={link.href} 
                            className='
                                flex 
                                w-full
                                flex-row 
                                items-start 
                                gap-3 
                                text-blue-500 
                                m-auto '>
                            <link.icon className='h-4 w-4'/>
                            <span>
                              {"managePermission" in link && link.managePermission
                                ? hasPermission(link.managePermission)
                                  ? `Manage ${link.title}`
                                  : link.title
                                : link.title}
                            </span>
                          </Link>
                        </Button>
                      
                    </SidebarMenuItem>
                    ))}
                  
                </SidebarMenu>
              </SidebarContent>
              <Separator />
              <SidebarFooter>
                
              </SidebarFooter>
              </div>
            </Sidebar>
          </SidebarProvider>
      
  )
}

export default AppSidebar
