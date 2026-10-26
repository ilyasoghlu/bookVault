

import { Section } from '@/types/section';

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  section: Section;
};


function SectionDetailsCard({
  section, 
}: Props ) {
  return (
   <Card>
      <CardHeader>
        <CardTitle>
          {section.title} 
        </CardTitle>
      </CardHeader>
        <p>
          <strong>Section number:</strong>{" "}
          {section.sectionnumber}
        </p>
      <CardContent className="space-y-3">
        <p>
          <strong>Floor:</strong>{" "}
          {section.sectionfloor}
        </p>
        <p>
          <strong>Capacity:</strong>{" "}
          {section.capacity}
        </p>
        <p>
          <strong>Description:</strong>{" "}
          {section.description}
        </p>
     
      </CardContent>
    </Card>
  )
}

export default SectionDetailsCard;
