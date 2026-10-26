
import { Employee } from '@/types/employee'

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


type Props = {
  employee: Employee;
};


function EmployeeDetailsCard({employee}: Props ) {
  return (
   <Card>
      <CardHeader>
        <CardTitle>
          {employee.firstname} {employee.lastname}
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-3">
        <p>
          <strong>Employee position:</strong>{" "}
          {employee.employeeposition}
        </p>
        <p>
          <strong>Hire date:</strong>{" "}
          {employee.hiredate}
        </p>
        {employee.enddate && (
          <p>
            <strong>End date:</strong>{" "}
            {employee.enddate}
          </p>
        )}
        <p>
          {employee.email}
        </p>
        <p>
          {employee.phone}
        </p>
      </CardContent>
    </Card>
  )
}

export default EmployeeDetailsCard
