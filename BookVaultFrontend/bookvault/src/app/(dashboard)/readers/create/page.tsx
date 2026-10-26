import React from 'react'


import CreateReaderForm from '@/components/readers/CreateReaderForm'

function CreateReaderPage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Register New Reader
      </h1>
      <p>Fill in the details below to register a new reader in the library database.</p>

      <CreateReaderForm mode={"create"} />
    </div>
  )
}

export default CreateReaderPage
