import {
  Archive,
  BookOpen,
  Briefcase,
  Building2,
  DoorOpen,
  FolderOpen,
  LayoutDashboard,
  Library,
  Repeat,
  UserRound,
  PenTool,
} from "lucide-react";

import { permissions } from "./permissions";

export const sidebarLinks = [
  {
    title: "Dashboard",
    href: "/dashboard",
    icon: LayoutDashboard,

  },
  {
    title: "Books",
    href: "/books",
    icon: BookOpen,
    permission: permissions.book.read,
    managePermission: permissions.book.update,
  },
  {
    title: "Authors",
    href: "/authors",
    icon: PenTool,
    permission: permissions.author.read,
    managePermission: permissions.author.update,
  },
  {
    title: "Publishers",
    href: "/publishers",
    icon: Building2,
    permission: permissions.publisher.read,
    managePermission: permissions.publisher.update,
  },
  {
    title: "Readers",
    href: "/readers",
    icon: UserRound,
    permission: permissions.reader.read,
    managePermission: permissions.reader.update,
  },
  {
    title: "Borrowings",
    href: "/borrowings",
    icon: Repeat,
    permission: permissions.borrowing.read,  
    managePermission: permissions.borrowing.update,
  },
  {
    title: "Employees",
    href: "/employees",
    icon: Briefcase,
    permission: permissions.employee.read,
    managePermission: permissions.employee.update,
  },
  {
    title: "Sections",
    href: "/sections",
    icon: FolderOpen,
    permission: permissions.section.read,
    managePermission: permissions.section.update,
  },
  {
    title: "Rooms",
    href: "/rooms",
    icon: DoorOpen,
    permission: permissions.room.read,
    managePermission: permissions.room.update,
  },
  {
    title: "Book Cases",
    href: "/bookcases",
    icon: Library,
    permission: permissions.bookCase.read,  
    managePermission: permissions.bookCase.update,
  },
  {
    title: "Shelves",
    href: "/shelves",
    icon: Archive,
    permission: permissions.shelf.read,
    managePermission: permissions.shelf.update,
  },
  {
    title: "Reports / Analytics",
    href: "/reports",
    icon: Archive,
    permission: permissions.shelf.read,
    managePermission: permissions.shelf.update,
  },
] as const;