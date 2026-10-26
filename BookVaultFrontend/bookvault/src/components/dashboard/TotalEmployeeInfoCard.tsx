 "use client"

import { Employee } from "@/types/employee"
import { UserRound } from "lucide-react"
import { Card, CardContent } from "../ui/card"
import { useEffect, useState } from "react" 
import { getEmployees } from "@/lib/employeeService"




function TotalEmployeeInfoCard() {
const [employees, setEmployees] = useState<Employee[]>([])
  useEffect(() => {
    const fetchEmployees = async () => {
      const data = await getEmployees()
      setEmployees(data)
    }
    fetchEmployees();
  }, []);

  const totalEmployees = employees.length;
  return (
    <Card>
      <CardContent className="flex flex-row items-center justify-center space-x-3">
        <div className="p-4 bg-blue-100 rounded-xl">
          <UserRound />
        </div>
        <div className="flex flex-col items-center">
          <h4 className="text-2xl font-bold">
                 Employee{totalEmployees > 1 && 's'}
          </h4>
          <h4 className="text-2xl font-bold text-blue-500">
            {totalEmployees}
          </h4>
        </div>
      </CardContent>
    </Card>
  )
}

export default TotalEmployeeInfoCard
