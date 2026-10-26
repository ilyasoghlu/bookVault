
"use client";

import { useEffect, useState } from "react";
import { getReaders } from "@/lib/readerService";
import { Reader } from "@/types/reader";
import ReaderCard from "@/components/readers/ReaderCard";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";
import DataView from "@/components/data-view/DataView";
import { DataViewConfig } from "@/components/data-view/types";
import DeleteReaderButton from "@/components/readers/DeleteReader";

export default function ReadersPage() {
  const [reader, setReader] = useState<Reader[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalReaders = reader.length;

  const bookConfig : DataViewConfig<Reader> ={
    columns: [
      {
        key:"firstname",
        label:"First name",
      },
      {
        key:"lastname",
        label:"Last name",
      },
      {
        key:"email",
        label:"Email"
      },
      {
        key:"id",
        label:"Actions",
        render:(reader: Reader) =>(
          <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/readers/details/${reader.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("READER_UPDATE") && (
        <Link href={`/readers/edit/${reader.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("READER_DELETE") && (
        <DeleteReaderButton
          id={reader.id}
          onDelete={handleDelete}
        />
      )}
    </div>
        )
      }

    ]
  }
  
  const handleDelete = (id:number) =>{
    setReader((prev)=> prev.filter((reader) => reader.id !==id))
  }
  
  useEffect(() => {
    const fetchReaders = async () => {
      try {
        const data = await getReaders();
        setReader(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Readers' information cant found.")
      }finally{
        setLoading(false)

      };
    };

    fetchReaders();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }
  return (
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalReaders} Reader{totalReaders>1&&'s'} found</h4>
        {hasPermission("READER_CREATE")&&(
          <Link href="/readers/create">
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
                Create reader
            </button>
          </Link>
        )}
      </div>
    
     {reader.length === 0 ? (
      <p>No books found.</p>
     ): (
      <DataView
        data={reader}
        config = {bookConfig}
        renderGridItem={(reader) =>(
          <ReaderCard
          reader={reader}
          onDelete={handleDelete}
          />
        )}
      />
     )}
    
    </div>
  );
}

