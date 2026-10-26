'use client'

import React from 'react'
import { useState, useEffect } from 'react';
import { BookCase } from '@/types/bookcase';
import CaseCard from '@/components/bookCase/CaseCard';
import { getBookCase } from '@/lib/bookCaseService';
import { useAuth } from '@/hooks/useAuth';
import Link from 'next/link';



function BookCasePage() {

  const [bookCase, setBookCase] = useState<BookCase[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalbookCases = bookCase.length;

 const handleDelete = (id:number) =>{
    setBookCase((prev)=> prev.filter((bookCase) => bookCase.id !==id))
  }


  useEffect(() =>{
    const fetchBookCase = async () =>{
      try {
        const data = await getBookCase()
        setBookCase(data)
      } catch (error) {
        console.log("Error", error)
        setError("Book case cant found.")
      }finally{
        setLoading(false)
      }
    }
    fetchBookCase()
  }, [])

   if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }

  return (
     <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalbookCases} Book Case{totalbookCases > 1 && 's'} found </h4>
         {hasPermission("BOOKCASE_CREATE")&&(
           <Link href="/bookcases/create">
          <button
            type="button"
            className="
              rounded-md 
              outline 
              hover:outline-blue-300 
               hover:text-orange-600
              border 
              px-3 
              py-2 
              text-sm"
            >
                Create Book Case
          </button>
        </Link>
         )}
      </div>
      

    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
      {bookCase.map((item) =>(
        <CaseCard
        key={item.id}
        bookCase={item}
        onDelete={handleDelete}
        />
      ))}
        
    </div>
    </div>
  )
}

export default BookCasePage
