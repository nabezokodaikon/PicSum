/* TODO: t_folder_view_counter ������������A�폜����B */
DELETE FROM t_folder_view_history
 WHERE file_id = (SELECT mf.file_id
                    FROM m_file mf
                   WHERE mf.file_path = :file_path
                 )