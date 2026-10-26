

"use client";

import { useEffect, useState } from "react";
import { getSections } from "@/lib/sectionService";
import { Section } from "@/types/section";
import SectionCard from "@/components/sections/SectionCard";
import DataView from "@/components/data-view/DataView";
import { DataViewConfig } from "@/components/data-view/types";
import { Button } from "@/components/ui/button";
import DeleteSectionButton from "@/components/sections/DeleteSection";
import Link from "next/link";
import { useAuth } from "@/hooks/useAuth";


export default function SectionssPage() {
  const [section, setSection] = useState<Section[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const {hasPermission} = useAuth();
  const totalSections = section.length;
  
  const sectionConfig: DataViewConfig<Section> = {
    columns:[
      {
        key:"title",
        label:"Section title",
      },
      {
        key:"sectionnumber",
        label: "Section number",
      },
      {
        key: "sectionfloor",
        label:"Section floor",
      },
      {
        key:"id",
        label: "Actions",
        render : (section: Section) =>(
          <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/sections/details/${section.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("SECTION_UPDATE") && (
        <Link href={`/sections/edit/${section.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("SECTION_DELETE") && (
        <DeleteSectionButton
          id={section.id}
          onDelete={handleDelete}
        />
      )}
    </div>

        )
      }
    ]
  }
  const handleDelete = (id:number) =>{
    setSection((prev)=> prev.filter((section) => section.id !==id))
  }
  
  useEffect(() => {
    const fetchSections = async () => {
      try {
        const data = await getSections();
        setSection(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Sections' information cant found.")
      }finally{
        setLoading(false)

      };
    };

    fetchSections();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }
  return (
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalSections} Section{totalSections>1&&'s'} found</h4>
      {hasPermission("SECTION_CREATE")&&(
        <Link href="/sections/create">
          <button
              type="button"
              className="
                rounded-md 
                outline 
                hover:outline-blue-300 
                hover:text-orange-600
                border 
                px-3 
                py-2 
                text-sm"
              >
              Create section
          </button>
        </Link>
      )}
      </div>
    
     {section.length === 0 ? (
      <p>No section found</p>
     ) : (
      <DataView
        data={section}
        config={sectionConfig}
        renderGridItem={(section) =>(
          <SectionCard
            section={section}
            onDelete={handleDelete}
          />
        )}
      />
     )}
    </div>
    
  );
}

