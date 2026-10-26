
"use client";

import { useEffect, useState } from "react";
import { getPublishers } from "@/lib/publisherService";
import { Publisher } from "@/types/publisher";
import PublisherCard from "@/components/publishers/PublisherCard";
import { Button } from "@/components/ui/button";
import DeletePublisherButton from "@/components/publishers/DeletePublisherButton";
import DataView from "@/components/data-view/DataView";
import { DataViewConfig } from "@/components/data-view/types";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";


export default function PublishersPage() {
  const [publishers, setPublishers] = useState<Publisher[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const{hasPermission} = useAuth();
  const totalPublishers = publishers.length;

  const publisherConfig : DataViewConfig<Publisher> = {
    columns:[
      {
        key: "publishername",
        label: "Publisher name",
      },
      {
        key:"address",
        label:"Address",
      },
      {
        key: "website",
        label: "Web site",
      },
      {
        key:"id",
        label: "Actions",
        render: (publishers: Publisher) => (

          <div className="flex items-center gap-2">
          {/* Details */}
          <Link href={`/publishers/details/${publishers.id}`}>
            <Button size="sm">
              View details
            </Button>
          </Link>

      {/* Edit */}
      {hasPermission("PUBLISHER_UPDATE") && (
        <Link href={`/publishers/edit/${publishers.id}`}>
          <Button size="sm" variant="outline">
            Edit
          </Button>
        </Link>
      )}

      {/* Delete */}
      {hasPermission("PUBLISHER_DELETE") && (
        <DeletePublisherButton
        id={publishers.id}
        onDelete={handleDelete}
        />
      
      )}
    </div>
        
    ),
  }
    ]
  }
  
  const handleDelete = (id:string) =>{
    setPublishers((prev)=> prev.filter((publisher) => publisher.id !==id))
  }
  
  useEffect(() => {
    const fetchPublishers = async () => {
      try {
        const data = await getPublishers();
        setPublishers(data);
      } catch (error) {
        console.error("Error:", error);
        setError("Publishers' information cant found.")
      }finally{
        setLoading(false)

      };
    };

    fetchPublishers();
  }, []);

  if (loading) {
    return <h2>Loading...</h2>;
    } if (error) {
    return <h2>{error}</h2>;
     }
  return (
    <div className="container mx-auto p-6">
      <div className="mb-6 flex items-center justify-between">
        <h4 className="text-2xl font-bold">{totalPublishers} Publisher{totalPublishers>1&&'s'} found</h4>
      {hasPermission("PUBLISHER_CREATE")&&(
        <Link href="/publishers/create">
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
              Create Publisher
          </button>
        </Link>
      )}
      </div>
   
     {publishers.length === 0 ? (
       <p>No publisher found</p>): (
      <DataView
        data={publishers}
        config = {publisherConfig}
        renderGridItem={(publishers) =>(
          <PublisherCard 
            publisher={publishers}
            onDelete={handleDelete}
          />
        )}
      />
     )}
    </div>
    
  );
}

