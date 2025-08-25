/*
This query retrieves a list of students along with the subjects they are enrolled in and the corresponding academic terms.
It joins the Enrollments table with Users, Subjects, and Terms tables to fetch the full name of the student, the subject name, and the term name.

Performance Notes:
- The query uses INNER JOINs, which are generally efficient if the joined columns (StudentId, SubjectId, TermId, and their referenced Ids) are indexed.
- If the Enrollments table is large, ensure that indexes exist on the foreign key columns (StudentId, SubjectId, TermId) to optimize join performance.
- The SELECT statement only retrieves three columns, minimizing data transfer.
- No WHERE clause is present, so the query will return all enrollments, which could impact performance on very large datasets.
- Consider adding pagination or filtering if the result set is expected to be large.
 */
SELECT
    u.Id AS StudentId,
    u.FullName AS StudentName,
    s.Id AS SubjectId,
    s.Name AS SubjectName,
    t.Id AS TermId,
    t.Name AS TermName
FROM
    Enrollments e
    INNER JOIN Users u ON e.StudentId = u.Id
    INNER JOIN Subjects s ON e.SubjectId = s.Id
    INNER JOIN Terms t ON e.TermId = t.Id;

select
    Id,
    FullName,
    SchoolId
from
    Users
where
    RoleId = 1;

select
    *
from
    Terms;