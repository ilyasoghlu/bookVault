"use client"

import { Reader } from '@/types/reader';
import DeleteReaderButton from './DeleteReader';

import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import { useAuth } from '@/hooks/useAuth';
import Link from 'next/link';

interface ReaderCardProps{
    reader :Reader;
    onDelete:(id:number) =>void
}

function ReaderCard({reader, onDelete} : ReaderCardProps) {
  const {hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle>
            {reader.firstname} {reader.lastname}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Reader bithday: {reader.dateofbirth}</strong>
        </p>
      
        <p>
            <strong>Membership expiration date: {reader.expirationdate}</strong>
        </p>
        <p className='mt-2'>{reader.email}</p>
        <p className='mt-2'>{reader.phone}</p>
          <Link href={`/readers/details/${reader.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("READER_UPDATE")&&(
            <Link href={`/readers/edit/${reader.id}`}>
              <Button>
                Edit reader
              </Button>
            </Link>
          )}
          {hasPermission("READER_DELETE")&&(
            <DeleteReaderButton 
                id={reader.id} 
                onDelete={onDelete}
                // fullname={`${author.firstname} ${author.lastname}`} 
                
                />
          )}
      </CardContent>
    </Card>
  )
}

export default ReaderCard
