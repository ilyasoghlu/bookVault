"use client";

import { useEffect, useState } from "react";
import { getAuthors } from "@/lib/authorService";
import { Author } from "@/types/author";
import AuthorCard from "@/components/authors/AuthorCard";
import DataView from "@/components/data-view/DataView";
import { Button } from "@/components/ui/button";
import DeleteAuthorButton from "@/components/authors/DeleteAuthorButton";
import { DataViewConfig } from "@/components/data-view/types";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";

export default function AuthorsPage() {
  const [authors, setAuthors] = useState<Author[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalAuthors = authors.length


  
  const authorConfig:DataViewConfig<Author> = {
    columns: [
      {
        key: "firstname",
        label: "Firstname"
      },
      {
        key:"lastname",
        label:"Lastname"
      },
      {
        key: "nationality",
        label: "Nationality"
      },
      {
        key: "id",
        label: "Actions",

        render: (author: Author) =>(
          <div className="flex items-center gap-2">
                    {/* Details */}
                    <Link href={`/authors/details/${author.id}`}>
                      <Button size="sm">
                        View details
                      </Button>
                    </Link>
          
                {/* Edit */}
                {hasPermission("AUTHOR_UPDATE") && (
                  <Link href={`/authors/edit/${author.id}`}>
                    <Button size="sm" variant="outline">
                      Edit
                    </Button>
                  </Link>
                )}
          
                {/* Delete */}
                {hasPermission("AUTHOR_DELETE") && (
                  <DeleteAuthorButton
                    id={author.id}
                    onDelete={handleDelete}
                  />
                )}
              </div>
            ),
      },
    ]
  }

  const handleDelete = (id:string) =>{
    setAuthors((prev)=> prev.filter((author) => author.id !==id))
  }
  
  useEffect(() => {
    const fetchAuthors = async () => {
      try {
        const data = await getAuthors();
        setAuthors(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Authors' information cant found.")
      }finally{
        setLoading(false)

      };
    };

    fetchAuthors();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }
  return (
    <div className="mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalAuthors} Author{totalAuthors > 1 && 's'} found </h4>
        {hasPermission("AUTHOR_CREATE")&&(
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
          <Link href="/authors/create">
            Create Author
          </Link>
        </button>
        )}
      </div>
     {authors.length === 0 ? (
      <p>No author found.</p>
     ): ( 
      <DataView
       data = {authors}
       config = {authorConfig}
       renderGridItem = {(author) =>(
         <AuthorCard 
         author ={author}
         onDelete={handleDelete}
         />
        )}
        />
      )}
    </div>
    
  );
}