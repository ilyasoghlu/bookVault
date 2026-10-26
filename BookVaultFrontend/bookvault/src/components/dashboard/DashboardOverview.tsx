import BookCategoryChart from "./BookCategoryChart"
import RecentActivity from "./RecentActivity"
import RestrictedMembers from "./RestrictedMembers"


function DashboardOverview() {
  return (
    <div className="grid  grid-cols-1 md:grid-cols-3 text-center gap-4">
      <BookCategoryChart books={[]} />
      <RecentActivity />
      <RestrictedMembers />
    </div>
  )
}

export default DashboardOverview
