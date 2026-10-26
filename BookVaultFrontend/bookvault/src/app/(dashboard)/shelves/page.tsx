

"use client";

import { useEffect, useState } from "react";
import { getShelves } from "@/lib/shelfService";
import { Shelf } from "@/types/shelf";
import ShelfCard from "@/components/shelves/ShelfCard";
import DataView from "@/components/data-view/DataView";
import { DataViewConfig } from "@/components/data-view/types";
import DeleteShelfButton from "@/components/shelves/DeleteShelf";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";


export default function ShelfPage() {
  const [shelf, setShelves] = useState<Shelf[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalShelves = shelf.length;
  
  const shelfConfig: DataViewConfig<Shelf> ={
    columns: [
      
      
      {
        key: "shelvecode",
        label: "Shelf code",
      },
      {
        key:"id",
        label:"Actions",
        render: (shelf :Shelf) =>(
          <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/shelves/details/${shelf.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("SHELF_UPDATE") && (
        <Link href={`/shelves/edit/${shelf.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("SHELF_DELETE") && (
        <DeleteShelfButton
          id={shelf.id}
          onDelete={handleDelete}
        />
      )}
    </div>
        )
      }
    ]
  }

  const handleDelete = (id:number) =>{
    setShelves((prev)=> prev.filter((shelf) => shelf.id !==id))
  }
  
  useEffect(() => {
    const fetchShelves = async () => {
      try {
        const data = await getShelves();
        setShelves(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Shelves' information cant found.")
      }finally{
        setLoading(false)

      };
    };

    fetchShelves();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }
  return (
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalShelves} Shelve{totalShelves>1&&'s'} found</h4>
      {hasPermission("SHELF_CREATE")&&(
        <Link href="/shelves/create">
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
              Create shelf
          </button>
        </Link>
      )}
      </div>
    
     {shelf.length === 0 ? (
      <p>No shelf found.</p>
     ) :(
      <DataView
        data={shelf}
        config={shelfConfig}
        renderGridItem={(shelf) =>(
          <ShelfCard
          shelf={shelf}
          onDelete={handleDelete}
          />
        )}
      />
     )}
    </div>
    
  );
}

