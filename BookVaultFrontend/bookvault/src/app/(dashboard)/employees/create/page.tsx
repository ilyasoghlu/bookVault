import CreateEmployeeForm from '@/components/employees/CreateEmployeeForm'
import React from 'react'


function CreateEmployeePage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Register New Employee
      </h1>
      <p>Fill in the details below to register a new employee in the library database.</p>

      <CreateEmployeeForm mode={"create"} />
    </div>
  )
}

export default CreateEmployeePage