import React from 'react'

import CreatePublisherForm from '@/components/publishers/CreatePublisherForm'

function CreatePublisherPage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Add New Publisher
      </h1>
      <p>Fill in the details below to add a new publisher to the library database.</p>

      <CreatePublisherForm mode={"create"} />
    </div>
  )
}

export default CreatePublisherPage
