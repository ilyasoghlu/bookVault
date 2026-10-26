import React from 'react'
import QuickActions from './DashboardQuickActions'
import TotalMembersInfoCard from './TotalMembersInfoCard'
import DashboardNavbar from './DashboardNavbar'
import TotalBooksInfoCard from './TotalBooksInfoCard'
import TotalBorrowedInfoCard from './TotalBorrowedInfoCard'
import TotalEmployeeInfoCard from './TotalEmployeeInfoCard'
import { Separator } from '../ui/separator'

function Dashboard() {
  return (
    <div>
      <DashboardNavbar />
      <Separator />
      <div className="grid grid-cols-1 mt-6 gap-4 md:grid-cols-4 ">
        <TotalBooksInfoCard />
        <TotalBorrowedInfoCard  />
        <TotalEmployeeInfoCard />
        <TotalMembersInfoCard />
      </div>
      <QuickActions mode='create' />
    </div>
  )
}

export default Dashboard
