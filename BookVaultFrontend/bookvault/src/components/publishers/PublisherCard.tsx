"use client"
import { Publisher } from '@/types/publisher';
import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import Link from 'next/link';
import { useAuth } from '@/hooks/useAuth';
import DeletePublisherButton from './DeletePublisherButton';

interface PublisherCardProps{
    publisher:Publisher;
    onDelete:(id:string) => void
}

function PublisherCard({publisher, onDelete}: PublisherCardProps ) {
  const {hasPermission} = useAuth();
  return (
    <Card>
      <CardHeader>
        <CardTitle>
            {publisher.publishername}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Publisher address: {publisher.address}</strong>
        </p>
        <p>
            <strong>City: {publisher.city}</strong>
        </p>
        <p>
            <strong>Email: {publisher.email}</strong>
        </p>
        <p>
            <strong>Phone: {publisher.phone}</strong>
        </p>
        <p>
            <strong>Website: {publisher.website}</strong>
        </p>
        
        
          <Link href={`/publishers/details/${publisher.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("PUBLISHER_UPDATE")&&(
            <Link href={`/publishers/edit/${publisher.id}`}>
              <Button>
                Edit publisher
              </Button>
            </Link>
          )}
          {hasPermission("PUBLISHER_DELETE")&&(
            <DeletePublisherButton 
                id={publisher.id} 
                onDelete={onDelete}
                // fullname={`${author.firstname} ${author.lastname}`} 
                
                />
          )}
      </CardContent>
    </Card>
  )
}

export default PublisherCard
