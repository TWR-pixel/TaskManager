-- Insert data into roles table
INSERT INTO public.roles (name) VALUES
  ('Admin'),
  ('Moderator'),
  ('User');

-- Insert test data into users table
INSERT INTO public.users (email_login, username, password_hash, password_salt, role_id) VALUES
  ('admin@example.com', 'admin', '$2b$10$i4jQ24f1y2gO02Kz9d5OqujV8G47w7f63sK64G/M6G7Y53dZ62c.', 'salt1', 2), -- Admin
  ('moderator@example.com', 'moderator', '$2b$10$i4jQ24f1y2gO02Kz9d5OqujV8G47w7f63sK64G/M6G7Y53dZ62c.', 'salt2', 2), -- Moderator
  ('user1@example.com', 'user1', '$2b$10$i4jQ24f1y2gO02Kz9d5OqujV8G47w7f63sK64G/M6G7Y53dZ62c.', 'salt3', 3), -- User
  ('user2@example.com', 'user2', '$2b$10$i4jQ24f1y2gO02Kz9d5OqujV8G47w7f63sK64G/M6G7Y53dZ62c.', 'salt4', 3); -- User

-- Note:
-- - The password_hash and password_salt values are placeholder examples. 
-- - You should use a secure password hashing library like bcrypt to generate real password hashes.
-- - Replace the placeholders with actual values based on your password hashing implementation.


-- Insert test data into task_columns table
INSERT INTO public.task_columns (name, description, owner_id) VALUES
  ('To Do', 'Tasks that are yet to be started.', 1),
  ('In Progress', 'Tasks that are currently being worked on.', 1),
  ('Done', 'Tasks that have been completed.', 1),
  ('Review', 'Tasks that need to be reviewed.', 2),
  ('Blocked', 'Tasks that are blocked and need attention.', 2);



-- Insert test data into tasks table
INSERT INTO public.tasks (title, content, "CreatedAt", is_completed, is_in_progress, owner_id, task_column_id) VALUES
  ('Write a blog post about task management', 'Research and write a comprehensive blog post about effective task management strategies.', '2023-11-01 10:00:00+00', false, true, 1, 1),
  ('Design a new website layout', 'Create a wireframe and mockups for a new website design.', '2023-11-01 14:30:00+00', false, false, 1, 1),
  ('Finalize presentation slides', 'Review and finalize presentation slides for the upcoming meeting.', '2023-11-02 09:00:00+00', true, false, 1, 3),
  ('Schedule team meeting', 'Schedule a team meeting to discuss project progress and next steps.', '2023-11-02 11:00:00+00', false, false, 2, 4),
  ('Fix bug in mobile app', 'Identify and fix a bug affecting the mobile application performance.', '2023-11-03 15:00:00+00', false, true, 2, 2);
