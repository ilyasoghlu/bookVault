

import CreateBookCaseForm from '@/components/bookCase/CreateBookCaseForm'

function CreateBookCasePage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Add New Book Case
      </h1>
      <p>Fill in the details below to add a new book case to the library database.</p>
      <CreateBookCaseForm mode={"create"} />
    </div>
  )
}

export default CreateBookCasePage
