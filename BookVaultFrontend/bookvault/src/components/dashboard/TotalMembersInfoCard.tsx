 "use client"
import { useEffect, useState } from "react"   
import { getReaders } from "@/lib/readerService"
import { Reader } from '@/types/reader'
import {  UsersRound } from "lucide-react"
import { Card, CardContent } from "../ui/card"




function TotalMembersInfoCard() {
const [readers, setReaders] = useState<Reader[]>([])

useEffect(() => {
  const fetchReaders = async () => {
    const data = await getReaders()
    setReaders(data)
  }
  fetchReaders();
}, []);

const totalReaders = readers.length;
  return (
    <Card>
      <CardContent className="flex flex-row items-center justify-center space-x-3">
        <div className="p-4 bg-blue-100 rounded-xl">
          <UsersRound />
        </div>
        <div className="flex flex-col items-center">
            <h4 className="text-2xl font-bold">
                  Total Reader{totalReaders > 1 && 's'}
            </h4>
            <h4 className="text-2xl font-bold text-blue-500">
              {totalReaders}
            </h4>
        </div>
      </CardContent>
    </Card  >
  )
}

export default TotalMembersInfoCard