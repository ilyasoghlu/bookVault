import React from 'react'

import CreateShelfForm from '@/components/shelves/CreateShelfForm'


function CreateShelfPage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Add New Shelf
      </h1>
      <p>Fill in the details below to add a new shelf to the library database.</p>

      <CreateShelfForm mode={"create"} />
    </div>
  )
}

export default CreateShelfPage
