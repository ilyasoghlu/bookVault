"use client"


import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getAuthorById } from '@/lib/authorService';
import AuthorDetailsDialog from '@/components/authors/AuthorDetailDialog';
import { Author } from '@/types/author';





function AuthorDetailsPage() {

    const params = useParams()
    const id = params.id as string;
    const router = useRouter();
    const [author, setAuthor] = useState<Author | null>(null)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string |null>(null);

    useEffect(() =>{
        const fetchPublisher = async() =>{
          try {
            const data = await getAuthorById(id);
            setAuthor(data)
          } catch (error) {
            console.error("Error", error);
            setError("Author information can't be found.")
          }finally{
            setLoading(false)
          }
        }


        fetchPublisher();
      }, [id]);

  if(loading) {
    return <div>Loading..</div>;
  }
  
  if(error || !author) {
    return <div>{error ?? "Author not found."}</div>
  }
  return (
    <>
     <AuthorDetailsDialog 
      author={author} 
      open={true} 
      onOpenChange={(open) => {
        if(!open) {
          router.push('/authors');
        }

      }} />
    </>
  )
}

export default AuthorDetailsPage
