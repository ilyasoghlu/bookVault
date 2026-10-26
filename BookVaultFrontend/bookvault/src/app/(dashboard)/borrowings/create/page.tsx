
import CreateBorrowingForm from "@/components/borrowings/CreateBorrowingForm";

function CreateBorrowingPage(){
    return (
        <div className="space-y-4 p-4">
          <h1 className="text-3xl font-bold">
            Add New Borrowing
          </h1>
          <p>Fill in the details below to add a new borrowing to the library database.</p>
        
          <CreateBorrowingForm mode={"create"} />
        </div>
    )
}


export default CreateBorrowingPage