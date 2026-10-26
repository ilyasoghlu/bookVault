"use client"

import DynamicTitle from '../global/DynamicTitle'

import { Sun, Moon, Bell, Globe } from 'lucide-react'
import { Button } from '../ui/button'
import { useAuth } from '@/hooks/useAuth'


function Navbar() {
  const iconBtn = "w-12 h-12 rounded-xl bg-gray-200 border-2 border-blue-50 shadow-sm text-blue-500 hover:bg-cyan-800 hover:text-white";
  const icons = [Sun, Moon, Globe, Bell];
  const {logout} = useAuth()
  return (
    <div className='
        flex 
        flex-col 
        sm:flex-row 
        items-center 
        justify-between 
        p-2 sm:ml-1 mt-1 
        bg-gray-100 w-full 
        sm:rounded-se-2xl 
        sm:rounded-ee-2xl 
        border-blue-50 
        border-2 shadow-sm'>
     <DynamicTitle />
  
      <div className='flex flex-row gap-3 my-3'>
        {icons.map((Icon, index) => (
        <Button
          key={index}
          className={iconBtn} variant="outline" size="icon"
        >
          <Icon className="mx-auto h-6 w-6" />
        </Button>
          ))}
        <Button className="w-36 h-12 rounded-xl bg-gray-200 border-2 border-blue-50 shadow-sm text-blue-500 hover:bg-cyan-800 hover:text-white" onClick={logout}>Log out</Button>  
      </div>
    </div>
  )
}
export default Navbar
