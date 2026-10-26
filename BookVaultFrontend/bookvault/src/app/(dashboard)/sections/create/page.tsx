import React from 'react'


import CreateSectionForm from '@/components/sections/CreateSectionForm'

function CreateSectionPage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Add New Section
      </h1>
      <p>Fill in the details below to add a new section to the library database.</p>

      <CreateSectionForm mode={"create"} />
    </div>
  )
}

export default CreateSectionPage
