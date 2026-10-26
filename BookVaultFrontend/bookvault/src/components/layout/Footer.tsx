import React from 'react'

function Footer() {
  return (
    <div className='p-3  bg-gray-100 w-full rounded-se-2xl rounded-ee-2xl border-blue-50 border-2 shadow-sm'>
      © 2026{new Date().getFullYear() > 2026 ? `-${new Date().getFullYear()}` : ""} BookVault · Library Management System
    </div>
  )
}

export default Footer
