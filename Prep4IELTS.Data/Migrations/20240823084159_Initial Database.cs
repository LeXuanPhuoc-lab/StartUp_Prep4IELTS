using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prep4IELTS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partition_Tag",
                columns: table => new
                {
                    partition_tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    partition_tag_desc = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartitionTag", x => x.partition_tag_id);
                });

            migrationBuilder.CreateTable(
                name: "System_Role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRole", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "Test_Category",
                columns: table => new
                {
                    test_category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    test_category_name = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCategory", x => x.test_category_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clerk_id = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    avatar_image = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    test_taken_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    target_score = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.role_id,
                        principalTable: "System_Role",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    test_title = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    test_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    total_engaged = table.Column<int>(type: "int", nullable: true),
                    total_question = table.Column<int>(type: "int", nullable: false),
                    total_section = table.Column<int>(type: "int", nullable: true),
                    test_category_id = table.Column<int>(type: "int", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.test_id);
                    table.ForeignKey(
                        name: "FK_Test_TestCategory",
                        column: x => x.test_category_id,
                        principalTable: "Test_Category",
                        principalColumn: "test_category_id");
                });

            migrationBuilder.CreateTable(
                name: "Flashcard",
                columns: table => new
                {
                    flashcard_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    total_words = table.Column<int>(type: "int", nullable: true),
                    total_view = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    isPublic = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcard", x => x.flashcard_id);
                    table.ForeignKey(
                        name: "FK_Flashcard_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    total_child_node = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parent_comment_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_Comment_ParentComment",
                        column: x => x.parent_comment_id,
                        principalTable: "Comment",
                        principalColumn: "comment_id");
                    table.ForeignKey(
                        name: "FK_Comment_Test",
                        column: x => x.test_id,
                        principalTable: "Test",
                        principalColumn: "test_id");
                    table.ForeignKey(
                        name: "FK_Comment_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Test_History",
                columns: table => new
                {
                    test_history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_right_answer = table.Column<int>(type: "int", nullable: true),
                    total_wrong_answer = table.Column<int>(type: "int", nullable: true),
                    total_skip_answer = table.Column<int>(type: "int", nullable: true),
                    total_question = table.Column<int>(type: "int", nullable: false),
                    total_completion_time = table.Column<int>(type: "int", nullable: false),
                    taken_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    accuracy_rate = table.Column<double>(type: "float", nullable: true),
                    isFull = table.Column<bool>(type: "bit", nullable: false),
                    test_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    band_score = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    test_category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestHistory", x => x.test_history_id);
                    table.ForeignKey(
                        name: "FK_TestHistory_Test",
                        column: x => x.test_id,
                        principalTable: "Test",
                        principalColumn: "test_id");
                    table.ForeignKey(
                        name: "FK_TestHistory_TestCategory",
                        column: x => x.test_category_id,
                        principalTable: "Test_Category",
                        principalColumn: "test_category_id");
                    table.ForeignKey(
                        name: "FK_TestHistory_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Test_Section",
                columns: table => new
                {
                    test_section_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    test_section_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    reading_desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    audio_resource_url = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: true),
                    total_question = table.Column<int>(type: "int", nullable: false),
                    section_transcript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSection", x => x.test_section_id);
                    table.ForeignKey(
                        name: "FK_TestSection_Test",
                        column: x => x.test_id,
                        principalTable: "Test",
                        principalColumn: "test_id");
                });

            migrationBuilder.CreateTable(
                name: "Test_Tag",
                columns: table => new
                {
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTag", x => new { x.test_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_TestTag_Tag",
                        column: x => x.tag_id,
                        principalTable: "Tag",
                        principalColumn: "tag_id");
                    table.ForeignKey(
                        name: "FK_TestTag_Test",
                        column: x => x.test_id,
                        principalTable: "Test",
                        principalColumn: "test_id");
                });

            migrationBuilder.CreateTable(
                name: "Flashcard_Detail",
                columns: table => new
                {
                    flashcard_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    word_text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    definition = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    word_form = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    word_pronunciation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    example = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    image_url = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: true),
                    flashcard_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardDetail", x => x.flashcard_detail_id);
                    table.ForeignKey(
                        name: "FK_FlashcardDetail_Flashcard",
                        column: x => x.flashcard_id,
                        principalTable: "Flashcard",
                        principalColumn: "flashcard_id");
                });

            migrationBuilder.CreateTable(
                name: "Test_Section_Partition",
                columns: table => new
                {
                    test_section_part_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    partition_desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_vertical_layout = table.Column<bool>(type: "bit", nullable: false),
                    partition_image = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: true),
                    test_section_id = table.Column<int>(type: "int", nullable: false),
                    partition_tag_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSectionPartition", x => x.test_section_part_id);
                    table.ForeignKey(
                        name: "FK_TestSectionPartition_Tag",
                        column: x => x.partition_tag_id,
                        principalTable: "Partition_Tag",
                        principalColumn: "partition_tag_id");
                    table.ForeignKey(
                        name: "FK_TestSectionPartition_TestSection",
                        column: x => x.test_section_id,
                        principalTable: "Test_Section",
                        principalColumn: "test_section_id");
                });

            migrationBuilder.CreateTable(
                name: "Partition_History",
                columns: table => new
                {
                    partition_history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    test_section_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    total_right_answer = table.Column<int>(type: "int", nullable: true),
                    total_wrong_answer = table.Column<int>(type: "int", nullable: true),
                    total_skip_answer = table.Column<int>(type: "int", nullable: true),
                    total_question = table.Column<int>(type: "int", nullable: false),
                    test_history_id = table.Column<int>(type: "int", nullable: false),
                    test_section_part_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartitionHistory", x => x.partition_history_id);
                    table.ForeignKey(
                        name: "FK_PartitionHistory_SectionPartition",
                        column: x => x.test_section_part_id,
                        principalTable: "Test_Section_Partition",
                        principalColumn: "test_section_part_id");
                    table.ForeignKey(
                        name: "FK_PartitionHistory_TestHistory",
                        column: x => x.test_history_id,
                        principalTable: "Test_History",
                        principalColumn: "test_history_id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_desc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    question_answer_explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    question_number = table.Column<int>(type: "int", nullable: false),
                    isMultipleChoice = table.Column<bool>(type: "bit", nullable: false),
                    test_section_part_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_Question_TestSectionPartition",
                        column: x => x.test_section_part_id,
                        principalTable: "Test_Section_Partition",
                        principalColumn: "test_section_part_id");
                });

            migrationBuilder.CreateTable(
                name: "Question_Answer",
                columns: table => new
                {
                    question_answer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    answer_display = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    answer_text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_true = table.Column<bool>(type: "bit", nullable: false),
                    question_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswer", x => x.question_answer_id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Question",
                        column: x => x.question_id,
                        principalTable: "Question",
                        principalColumn: "question_id");
                });

            migrationBuilder.CreateTable(
                name: "Test_Grade",
                columns: table => new
                {
                    test_grade_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grade_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    question_number = table.Column<int>(type: "int", nullable: false),
                    right_answer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    inputed_answer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    question_id = table.Column<int>(type: "int", nullable: false),
                    partition_history_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestGrade", x => x.test_grade_id);
                    table.ForeignKey(
                        name: "FK_TestGrade_PartitionHistory",
                        column: x => x.partition_history_id,
                        principalTable: "Partition_History",
                        principalColumn: "partition_history_id");
                    table.ForeignKey(
                        name: "FK_TestGrade_Question",
                        column: x => x.question_id,
                        principalTable: "Question",
                        principalColumn: "question_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_parent_comment_id",
                table: "Comment",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_test_id",
                table: "Comment",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_user_id",
                table: "Comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_user_id",
                table: "Flashcard",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_Detail_flashcard_id",
                table: "Flashcard_Detail",
                column: "flashcard_id");

            migrationBuilder.CreateIndex(
                name: "IX_Partition_History_test_history_id",
                table: "Partition_History",
                column: "test_history_id");

            migrationBuilder.CreateIndex(
                name: "IX_Partition_History_test_section_part_id",
                table: "Partition_History",
                column: "test_section_part_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_test_section_part_id",
                table: "Question",
                column: "test_section_part_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Answer_question_id",
                table: "Question_Answer",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_test_category_id",
                table: "Test",
                column: "test_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Grade_partition_history_id",
                table: "Test_Grade",
                column: "partition_history_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Grade_question_id",
                table: "Test_Grade",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_History_test_category_id",
                table: "Test_History",
                column: "test_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_History_test_id",
                table: "Test_History",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_History_user_id",
                table: "Test_History",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Section_test_id",
                table: "Test_Section",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Section_Partition_partition_tag_id",
                table: "Test_Section_Partition",
                column: "partition_tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Section_Partition_test_section_id",
                table: "Test_Section_Partition",
                column: "test_section_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Tag_tag_id",
                table: "Test_Tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User__EA2ECA192588F56C",
                table: "User",
                column: "clerk_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Flashcard_Detail");

            migrationBuilder.DropTable(
                name: "Question_Answer");

            migrationBuilder.DropTable(
                name: "Test_Grade");

            migrationBuilder.DropTable(
                name: "Test_Tag");

            migrationBuilder.DropTable(
                name: "Flashcard");

            migrationBuilder.DropTable(
                name: "Partition_History");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Test_History");

            migrationBuilder.DropTable(
                name: "Test_Section_Partition");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Partition_Tag");

            migrationBuilder.DropTable(
                name: "Test_Section");

            migrationBuilder.DropTable(
                name: "System_Role");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Test_Category");
        }
    }
}
