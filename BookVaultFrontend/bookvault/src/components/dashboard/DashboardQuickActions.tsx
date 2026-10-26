
"use client"

// import { Separator } from '../ui/separator'
import { useState } from 'react'
import CreateBookForm from '../books'
import CreateReaderForm from '../readers'
import CreateBorrowingForm from '../borrowings'
import CreateEmployeeForm from '../employees'
import DashboardOverview from './DashboardOverview'
import { Reader } from '@/types/reader'
import ReaderList from '../readers/ReaderList'
import Link from 'next/link'


type Props = {
  mode: 'create' | 'edit'
  reader?: Reader
}

function QuickActions({ 
    mode, 
    reader 
  }: Props) {
  

  const [activeAction, setActiveAction] = useState("Overview")
  const isActive = (action: string) => {
    return action === activeAction
  }

  const tabs = [
    "Overview", 
    "Add New Book", 
    "Register New Reader", 
    "Manage Borrowings", 
    "Manage Employees"
  ]

const renderTabContent = () => {
  if(activeAction === "Overview") {
    return (
      <div>
        <DashboardOverview />
      </div>
    )
  }
  if(activeAction === "Add New Book") {
    return (
      <div>
        <CreateBookForm mode='create'/>
      </div>
    )
  }
  if(activeAction === "Register New Reader") {
    if (mode==="create") {
     return (
      <div>
        <Link href="/readers/update">
        <button
          type="button"
        >
          Update Reader 
        </button>
        </Link>
        <CreateReaderForm mode='create'/>
      </div>
     )
    }
    return (
      <>
      <div className='border-2 p-4 mt-4 rounded-xl grid grid-cols-1 md:grid-cols-2 gap-4'>
        <CreateReaderForm mode='edit'/>
         <div className='border-l-2 hidden md:block'>
          <ReaderList reader={reader}/>
          {/* <DataView 
            data = {readersData}
            config = {readersConfig}
            
            /> */}

          Something will be displayed here.
        </div>
      </div>
            </>
    )
  }
  if(activeAction === "Manage Borrowings") {
    return (
      <div>
        <CreateBorrowingForm mode='create'/>
      </div>
    )
  }
  if(activeAction === "Manage Employees") {
    return (
      <div>
        <CreateEmployeeForm mode='create'/>
      </div>
    )
  }
  return null
}

  return (
    <div className="mt-10">
      <h2 className="text-2xl font-bold">
        Quick Actions
      </h2>
      <div className="flex flex-col md:flex-row space-x-0 md:space-x-6 border-2 rounded-xl">
        {
          tabs.map((tab) => (
            <div key={tab} className={`cursor-pointer m-4 p-4 ${isActive(tab) ? "border-b-4 border-b-blue-500" : ""}`} onClick={() => setActiveAction(tab)}>
              {tab}
            </div>
          ))
        } 
      </div>
    
      <div>
        {renderTabContent()}
      </div>
       
    </div>
    
  )
}

export default QuickActions