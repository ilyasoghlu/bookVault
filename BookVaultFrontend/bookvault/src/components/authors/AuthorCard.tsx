"use client"

import { Author } from '@/types/author';
import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'
import { Button } from '../ui/button';
import Link from 'next/link';
import { useAuth } from '@/hooks/useAuth';
import DeleteAuthorButton from './DeleteAuthorButton';
import { SquarePen, Eye} from "lucide-react";


interface AuthorCardProps{
    author:Author;
    onDelete:(id:string) => void
}

function AuthorCard({author, onDelete}: AuthorCardProps ) {
  const {hasPermission} = useAuth();
  return (
    <Card>
      <CardHeader>
        <CardTitle>
            {author.firstname} {author.lastname}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Birthday: {author.birthdate}</strong>
        </p>
        <p>
            <strong>Nationality: {author.nationality}</strong>
        </p>
        <p className='mt-2'>{author.biography}</p>
          <Link href={`/authors/details/${author.id}`}>
            <Button>
              <Eye />
              View details
            </Button>
          </Link>
          {hasPermission("AUTHOR_UPDATE")&&(
            <Link href={`/authors/edit/${author.id}`}>
              <Button>
                <SquarePen />
                Edit author
              </Button>
            </Link>
          )}
          {hasPermission("AUTHOR_DELETE")&&(
            <DeleteAuthorButton 
              id={author.id} 
              onDelete={onDelete}
              // fullname={`${author.firstname} ${author.lastname}`}  
              />
          )}
          
      </CardContent>
    </Card>
  )
}

export default AuthorCard
