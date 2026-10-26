"use client"
import { Shelf } from '@/types/shelf';
import DeleteShelfButton from './DeleteShelf';


import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import { useAuth } from '@/hooks/useAuth';
import Link from 'next/link';

interface SectionCardProps{
    shelf :Shelf;
    onDelete:(id:number) =>void
}

function ShelfCard({shelf, onDelete} : SectionCardProps) {
  const {hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle>
            {shelf.shelvecode} 
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Capacity: </strong>
            {shelf.capacity}
        </p>
        
        
          <Link href={`/shelves/details/${shelf.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("SHELF_UPDATE")&&(
            <Link href={`/shelves/edit/${shelf.id}`}>
              <Button>
                Edit shelf
              </Button>
            </Link>
          )}
          {hasPermission("SHELF_DELETE")&&(
            <DeleteShelfButton 
              id={shelf.id} 
              onDelete={onDelete}
              // fullname={`${author.firstname} ${author.lastname}`} 
            />
          )}
      </CardContent>
    </Card>
  )
}

export default ShelfCard
