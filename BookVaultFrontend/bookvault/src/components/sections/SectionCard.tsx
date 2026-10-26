"use client"
import { Section } from '@/types/section';
import DeleteSectionButton from './DeleteSection';


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
    section :Section;
    onDelete:(id:number) =>void
}

function SectionCard({section, onDelete} : SectionCardProps) {
  const {hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle>
            {section.title} 
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Floor: </strong>
            {section.sectionfloor}
        </p>
        <p>
            <strong>Capacity: </strong>
            {section.capacity}
        </p>
        <p>
            <strong>Description:</strong>
            {section.description}
        </p>
        
          <Link href={`/sections/details/${section.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("SECTION_UPDATE")&&(
            <Link href={`/sections/edit/${section.id}`}>
              <Button>
                Edit section
              </Button>
            </Link>
          )}
          {hasPermission("SECTION_DELETE")&&(
            <DeleteSectionButton 
              id={section.id} 
              onDelete={onDelete}
              // fullname={`${author.firstname} ${author.lastname}`} 
            
            />
          )}
      </CardContent>
    </Card>
  )
}

export default SectionCard
