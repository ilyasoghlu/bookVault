export const permissions = {
  book: {
    read: "BOOK_READ",
    create: "BOOK_CREATE",
    update: "BOOK_UPDATE",
    delete: "BOOK_DELETE",
  },

  author: {
    read: "AUTHOR_READ",
    create: "AUTHOR_CREATE",
    update: "AUTHOR_UPDATE",
    delete: "AUTHOR_DELETE",
  },

  publisher: {
    read: "PUBLISHER_READ",
    create: "PUBLISHER_CREATE",
    update: "PUBLISHER_UPDATE",
    delete: "PUBLISHER_DELETE",
  },

  bookCase: {
    read: "BOOKCASE_READ",
    create: "BOOKCASE_CREATE",
    update: "BOOKCASE_UPDATE",
    delete: "BOOKCASE_DELETE",
  },

  room: {
    read: "ROOM_READ",
    create: "ROOM_CREATE",
    update: "ROOM_UPDATE",
    delete: "ROOM_DELETE",
  },

  section: {
    read: "SECTION_READ",
    create: "SECTION_CREATE",
    update: "SECTION_UPDATE",
    delete: "SECTION_DELETE",
  },

  shelf: {
    read: "SHELF_READ",
    create: "SHELF_CREATE",
    update: "SHELF_UPDATE",
    delete: "SHELF_DELETE",
  },

  employee: {
    read: "EMPLOYEE_READ",
    create: "EMPLOYEE_CREATE",
    update: "EMPLOYEE_UPDATE",
    delete: "EMPLOYEE_DELETE",
  },

  reader: {
    read: "READER_READ",
    create: "READER_CREATE",
    update: "READER_UPDATE",
    delete: "READER_DELETE",
  },

  borrowing: {
    read: "BORROWING_READ",
    create: "BORROWING_CREATE",
    update: "BORROWING_UPDATE",
  },

  rule: {
    create: "RULE_CREATE",
    assign: "RULE_ASSIGN",
  },
} as const;