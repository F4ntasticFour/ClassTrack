-- Academic Advisor Table
CREATE TABLE academic_advisor (
                                  advisor_id   INTEGER ,
                                  name         VARCHAR(255) NOT NULL,
                                  email        VARCHAR(255) UNIQUE,
                                  PRIMARY KEY (advisor_id)
);

-- Instructor Table
CREATE TABLE instructor (
                            instructor_id INTEGER,
                            name          VARCHAR(255) NOT NULL,
                            email         VARCHAR(255) UNIQUE,
                            PRIMARY KEY (instructor_id)
);

-- Course Table
CREATE TABLE course (
                        course_code  VARCHAR(255),
                        title        VARCHAR(255),
                        semester     VARCHAR(50),
                        PRIMARY KEY (course_code, semester)
);

-- Student Table
CREATE TABLE student (
                         student_id  INTEGER,
                         name        VARCHAR(255),
                         gender      VARCHAR(50),
                         email       VARCHAR(255) UNIQUE,
                         advisor_id  INTEGER,
                         PRIMARY KEY (student_id),
                         FOREIGN KEY (advisor_id) REFERENCES academic_advisor (advisor_id)
);

-- Enrollment Table
CREATE TABLE enroll (
                        student_id   INTEGER,
                        course_code  VARCHAR(255),
                        semester     VARCHAR(50),
                        FOREIGN KEY (student_id) REFERENCES student (student_id),
                        FOREIGN KEY (course_code, semester) REFERENCES course (course_code, semester)
);

-- Teaching Assignment Table
CREATE TABLE teach (
                       instructor_id INTEGER,
                       course_code   VARCHAR(255),
                       semester      VARCHAR(50),
                       FOREIGN KEY (instructor_id) REFERENCES instructor (instructor_id),
                       FOREIGN KEY (course_code, semester) REFERENCES course (course_code, semester)
);

-- Course Section Table
CREATE TABLE course_section (
                                section_id    INTEGER,
                                instructor_id INTEGER,
                                course_code   VARCHAR(255),
                                semester      VARCHAR(50),
                                PRIMARY KEY (section_id),
                                FOREIGN KEY (instructor_id) REFERENCES instructor (instructor_id),
                                FOREIGN KEY (course_code, semester) REFERENCES course (course_code, semester)
);

-- Class Session Table
CREATE TABLE class_session (
                               session_id   INTEGER,
                               room         VARCHAR(255),
                               date         DATE,
                               time_slot    TIME,
                               section_id   INTEGER,
                               PRIMARY KEY (session_id),
                               FOREIGN KEY (section_id) REFERENCES course_section (section_id)
);

-- Attendance Record Table
CREATE TABLE attendance_record (
                                   record_id          INTEGER,
                                   attendance_value   BIT,
                                   section_id         INTEGER,
                                   student_id         INTEGER,
                                   session_id         INTEGER,
                                   PRIMARY KEY (record_id),
                                   FOREIGN KEY (section_id) REFERENCES course_section (section_id),
                                   FOREIGN KEY (student_id) REFERENCES student (student_id),
                                   FOREIGN KEY (session_id) REFERENCES class_session (session_id)
);

-- Attendance Report Table
CREATE TABLE attendance_report (
                                   report_id       INTEGER,
                                   instructor_id   INTEGER,
                                   section_id      INTEGER,
                                   record_id       INTEGER,
                                   PRIMARY KEY (report_id),
                                   FOREIGN KEY (instructor_id) REFERENCES instructor (instructor_id),
                                   FOREIGN KEY (section_id) REFERENCES course_section (section_id),
                                   FOREIGN KEY (record_id) REFERENCES attendance_record (record_id)
);

-- Support Table
CREATE TABLE support (
                         support_id  INTEGER,
                         student_id  INTEGER,
                         PRIMARY KEY (support_id),
                         FOREIGN KEY (student_id) REFERENCES student (student_id)
);
