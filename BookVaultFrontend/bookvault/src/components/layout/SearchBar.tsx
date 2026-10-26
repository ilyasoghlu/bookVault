import React from 'react'
import { Search } from 'lucide-react'

function SearchBar() {
  return (
    <div className="relative w-full max-w-md">
        <Search className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400"
      />
        <input className='
          w-full rounded-lg border 
          border-gray-300 
          bg-white 
          py-2 
          pl-10 
          pr-4
          text-sm 
          text-gray-900
          placeholder:text-gray-400
          shadow-sm 
          outline-none
          transition
          focus:border-blue-500
          focus:ring-2 focus:ring-blue-200
          dark:border-gray-700
          dark:bg-gray-900
          dark:text-white
          dark:focus:border-blue-400'
          type='search'  placeholder="type here and search..." />       
     </div>
  )
}

export default SearchBar
