"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { getSectionById } from '@/lib/sectionService';
import SectionDetailDialog from '@/components/sections/SectionDetailDialog';
import { Section } from '@/types/section';



function SectionDetailsPage() {

const params = useParams()
  const id = Number(params.id);
  const router = useRouter();
  const [section, setSection] = useState<Section | null>(null)
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string |null>(null);

  useEffect(() =>{
    const fetchSection = async() =>{
      try {
        const data = await getSectionById(id);
        setSection(data)
      } catch (error) {
        console.error("Error", error);
        setError("Section information can't be found.")
      }finally{
        setLoading(false)
      }
    }
    fetchSection();
  }, [id]);

  if(loading) {
    return <div>Loading..</div>;
  }
  
  if(error || !section) {
    return <div>{error ?? "Section not found."}</div>
  }

  return (
    <>
     <SectionDetailDialog 
        section={section} 
        open={true} 
        onOpenChange={(open) => {
          if (!open) {
            router.push('/sections');
          }
        }} 
     />
    </>
  )
}

export default SectionDetailsPage