import React from 'react'
import CreateAuthorForm from '@/components/authors/CreateAuthorForm'

function CreateAuthorPage() {
  return (
    <div className="space-y-4 p-4 ">
      <h1 className="text-3xl font-bold">
        Add New Author
      </h1>
      <p>Fill in the details below to add a new author to the library database.</p>
      <CreateAuthorForm mode={"create"} />
    </div>
  )
}

export default CreateAuthorPage
