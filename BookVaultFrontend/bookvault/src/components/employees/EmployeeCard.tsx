"use client"

import { Employee } from '@/types/employee'
import DeleteEmployeeButton from './DeleteEmployeeButton';

import { 
    Card,
    CardContent,
    CardHeader,
    CardTitle
} from '../ui/card'

import { Button } from '../ui/button';
import { useAuth } from '@/hooks/useAuth';
import Link from 'next/link';

interface EmployeeCardProps{
    employee :Employee;
    onDelete:(id:number) =>void
}

function EmployeeCard({employee, onDelete} : EmployeeCardProps) {
  const {hasPermission} = useAuth();
  return (
     <Card>
      <CardHeader>
        <CardTitle>
            {employee.firstname} {employee.lastname}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p>
            <strong>Employee position: {employee.employeeposition}</strong>
        </p>
        <p>
            <strong>Hire date: {employee.hiredate}</strong>
        </p>
        <p className='mt-2'>{employee.email}</p>
        <p className='mt-2'>{employee.phone}</p>
          <Link href={`/employees/details/${employee.id}`}>
            <Button>
              View details
            </Button>
          </Link>
          {hasPermission("EMPLOYEE_UPDATE")&&(
            <Link href={`/employees/edit/${employee.id}`}>
            <Button>
              Edit employee
            </Button>
          </Link>
          )}
          {hasPermission("EMPLOYEE_DELETE")&&(
            <DeleteEmployeeButton 
            id={employee.id} 
            onDelete={onDelete}
            // fullname={`${author.firstname} ${author.lastname}`} 
            
            />
          )}
      </CardContent>
    </Card>
  )
}

export default EmployeeCard
