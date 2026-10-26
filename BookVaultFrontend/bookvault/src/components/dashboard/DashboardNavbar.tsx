import React from 'react'
import SearchBar from '../layout/SearchBar'
import UserAccount from './UserAccount'

function DashboardNavbar() {
  return (
    <div className="flex items-center justify-between py-6">
      <UserAccount />
      <SearchBar />
    </div>
  )
}

export default DashboardNavbar
