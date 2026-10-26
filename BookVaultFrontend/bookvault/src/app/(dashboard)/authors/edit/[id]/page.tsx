"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getAuthorById } from '@/lib/authorService'
import { Author } from '@/types/author';
import AuthorUpdateDialog from '@/components/authors/AuthorUpdateDialog'



function EditAuthorDetails() {
  const router = useRouter();
  const params = useParams();
  const id = params.id as string;
  const [author, setAuthor] = useState<Author | null>(null);
  const [loading, setLoading] = useState(true);

     useEffect(() => {
          const fetchAuthor = async()=>{
            try {
              const data = await getAuthorById(id);
              setAuthor(data);
            } catch (error) {
              console.error("Error:", error);
    
            }finally{
              setLoading(false);
            }
          }
          fetchAuthor()
        }, [id]);


         if (loading) {
    return <div>Loading...</div>;
  }

  if (!author) {
    return <div>Author not found.</div>;
  }
  return (

    <AuthorUpdateDialog 
      author={author} 
      open={true} 
      onOpenChange={(open) => {
        if (!open) {
          router.push('/authors');
        }
      }}
    />

  )
}

export default EditAuthorDetails
