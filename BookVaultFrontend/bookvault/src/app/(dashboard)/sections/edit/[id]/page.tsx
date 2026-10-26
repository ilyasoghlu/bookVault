"use client"

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import SectionUpdateDialog from '@/components/sections/SectionUpdateDialog'
import { getSectionById } from '@/lib/sectionService'
import { Section } from '@/types/section';



function EditSectionDetails() {

   const params = useParams()
   const id = Number(params.id)
   const router = useRouter();
   const [section, setSection] = useState<Section | null>(null);
   const [loading, setLoading] = useState(true);
   
       useEffect(() => {
         const fetchSection = async()=>{
           try {
             const data = await getSectionById(id);
             setSection(data);
           } catch (error) {
             console.error("Error:", error);
   
           }finally{
             setLoading(false);
           }
         }
         fetchSection()
       }, [id]);
      
        if (loading) {
       return <div>Loading...</div>;
     }
   
     if (!section) {
       return <div>Section not found.</div>;
     } 

  return (
    <SectionUpdateDialog 
        section={section} 
        open={true} 
        onOpenChange={(open) => {
          if (!open) {
            router.push('/sections');
          }
        }} 
      />

  )
}

export default EditSectionDetails
