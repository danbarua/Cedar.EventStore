INSERT INTO events(stream_id_internal, stream_version, id, created, type, json_data, json_metadata)
VALUES (:stream_id_internal, :stream_version, :id, :created, :type, :json_data, :json_metadata);