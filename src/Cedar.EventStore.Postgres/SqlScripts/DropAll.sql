DROP TABLE IF EXISTS events;
DROP TABLE IF EXISTS streams;
DROP SEQUENCE IF EXISTS streams_id_internal_seq;
DROP SEQUENCE IF EXISTS events_ordinal_seq;

--drop stream version sequences
DO $$DECLARE r record;
BEGIN

	FOR r IN 
		SELECT quote_ident(c.relname) as relname
		FROM pg_class c
		WHERE c.relkind = 'S'
		AND c.relname LIKE 'stream_version_%'
	LOOP
		EXECUTE 'DROP SEQUENCE ' || r.relname || ';';
	END LOOP;
END$$;
