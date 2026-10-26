"use client"
import { useAuth } from '@/hooks/useAuth' 
import { useEffect, useState } from 'react'

const getTime = (date: Date, options?: Intl.DateTimeFormatOptions) => {
  return date.toLocaleTimeString([], options)
}

function UserAccount() {
    const { user } = useAuth()
    const [currentTime, setCurrentTime] = useState(getTime(new Date(), {day: '2-digit', month:"short", year: 'numeric', hour: '2-digit', minute: '2-digit' }))

    useEffect(() => {
        const interval = setInterval(() => {
            setCurrentTime(getTime(new Date(), {day: '2-digit', month:"short", year: 'numeric', hour: '2-digit', minute: '2-digit' }))
        }, 60000)
        return () => clearInterval(interval)
    }, [])

  return (
    <div className="flex flex-col items-start space-y-1">
      <h3 className="text-lg font-semibold">Welcome, <span className="text-sm font-medium capitalize" >{user?.name}</span> !</h3>
       
      <p className="text-sm font-bold text-blue-500">{currentTime}</p>    
    </div>
  )
}

export default UserAccount
