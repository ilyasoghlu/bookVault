import React from 'react'


import CreateRoomForm from '@/components/rooms/CreateRoomForm'

function CreateRoomPage() {
  return (
    <div className="space-y-4 p-4">
      <h1 className="text-3xl font-bold">
        Add New Room
      </h1>
      <p>Fill in the details below to add a new room to the library database.</p>
      
      <CreateRoomForm mode={"create"} />
    </div>
  )
}

export default CreateRoomPage
