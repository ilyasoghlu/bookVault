import React from 'react'

import CreateBookForm from '@/components/books/CreateBookForm'


function CreateBookPage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Add New book
      </h1>
      <p>Fill in the details below to add a new book to the library database.</p>

      <CreateBookForm mode={"create"} />
    </div>
  )
}

export default CreateBookPage