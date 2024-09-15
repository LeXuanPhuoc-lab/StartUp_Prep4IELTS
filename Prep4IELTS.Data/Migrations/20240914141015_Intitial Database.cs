using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prep4IELTS.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cloud_Resource",
                columns: table => new
                {
                    cloud_resource_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    public_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false),
                    bytes = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudResource", x => x.cloud_resource_id);
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
                    isPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcard", x => x.flashcard_id);
                });

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
                name: "Payment_Type",
                columns: table => new
                {
                    payment_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_method = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.payment_type_id);
                });

            migrationBuilder.CreateTable(
                name: "Premium_Package",
                columns: table => new
                {
                    premium_package_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    premium_package_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    duration_in_months = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumPackage", x => x.premium_package_id);
                });

            migrationBuilder.CreateTable(
                name: "Score_Calculation",
                columns: table => new
                {
                    score_calculation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_total_right = table.Column<int>(type: "int", nullable: false),
                    to_total_right = table.Column<int>(type: "int", nullable: false),
                    test_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    band_score = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCalculation", x => x.score_calculation_id);
                });

            migrationBuilder.CreateTable(
                name: "Speaking_Topic",
                columns: table => new
                {
                    topic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    topic_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakingTopic", x => x.topic_id);
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
                name: "Speaking_Topic_Sample",
                columns: table => new
                {
                    topic_sample_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    topic_sample_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    topic_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakingTopicSample", x => x.topic_sample_id);
                    table.ForeignKey(
                        name: "FK_SpeakingTopicSample_Topic",
                        column: x => x.topic_id,
                        principalTable: "Speaking_Topic",
                        principalColumn: "topic_id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Speaking_Part",
                columns: table => new
                {
                    part_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    part_number = table.Column<int>(type: "int", nullable: false),
                    part_description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    topic_sample_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakingPart", x => x.part_id);
                    table.ForeignKey(
                        name: "FK_SpeakingPart_Sample",
                        column: x => x.topic_sample_id,
                        principalTable: "Speaking_Topic_Sample",
                        principalColumn: "topic_sample_id",
                        onDelete: ReferentialAction.Cascade);
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
                    is_draft = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Test_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "User_Flashcard",
                columns: table => new
                {
                    user_flashcard_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    flashcard_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlashcard", x => x.user_flashcard_id);
                    table.ForeignKey(
                        name: "FK_UserFlashcard_Flashcard",
                        column: x => x.flashcard_id,
                        principalTable: "Flashcard",
                        principalColumn: "flashcard_id");
                    table.ForeignKey(
                        name: "FK_UserFlashcard_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "User_Premium_Package",
                columns: table => new
                {
                    user_premium_package_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    premium_package_id = table.Column<int>(type: "int", nullable: false),
                    expire_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPremiumPackage", x => x.user_premium_package_id);
                    table.ForeignKey(
                        name: "FK_UserPremiumPackage_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "PK_UserPremiumPackage_PremiumPackage",
                        column: x => x.premium_package_id,
                        principalTable: "Premium_Package",
                        principalColumn: "premium_package_id");
                });

            migrationBuilder.CreateTable(
                name: "User_Speaking_Sample_History",
                columns: table => new
                {
                    user_sample_history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    topic_sample_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpeakingSampleHistory", x => x.user_sample_history_id);
                    table.ForeignKey(
                        name: "FK_UserSpeakingSampleHistory_Sample",
                        column: x => x.topic_sample_id,
                        principalTable: "Speaking_Topic_Sample",
                        principalColumn: "topic_sample_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpeakingSampleHistory_User",
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
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    score_calculation_id = table.Column<int>(type: "int", nullable: true),
                    test_category_id = table.Column<int>(type: "int", nullable: false),
                    is_resubmitted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestHistory", x => x.test_history_id);
                    table.ForeignKey(
                        name: "FK_TestHistory_ScoreCaculation",
                        column: x => x.score_calculation_id,
                        principalTable: "Score_Calculation",
                        principalColumn: "score_calculation_id");
                    table.ForeignKey(
                        name: "FK_TestHistory_Test",
                        column: x => x.test_id,
                        principalTable: "Test",
                        principalColumn: "test_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestHistory_TestCategory",
                        column: x => x.test_category_id,
                        principalTable: "Test_Category",
                        principalColumn: "test_category_id");
                    table.ForeignKey(
                        name: "FK_TestHitory_User",
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
                    total_question = table.Column<int>(type: "int", nullable: false),
                    section_transcript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    test_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cloud_resource_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSection", x => x.test_section_id);
                    table.ForeignKey(
                        name: "FK_TestSection_CloudResource",
                        column: x => x.cloud_resource_id,
                        principalTable: "Cloud_Resource",
                        principalColumn: "cloud_resource_id");
                    table.ForeignKey(
                        name: "FK_TestSection_Test",
                        column: x => x.test_id,
                        principalTable: "Test",
                        principalColumn: "test_id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "User_Flashcard_Progress",
                columns: table => new
                {
                    user_flashcard_progress_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    progress_status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    user_flashcard_id = table.Column<int>(type: "int", nullable: false),
                    flashcard_detail_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlashcardProgress", x => x.user_flashcard_progress_id);
                    table.ForeignKey(
                        name: "FK_UserFlashcardProgress_FlashcardDetail",
                        column: x => x.flashcard_detail_id,
                        principalTable: "Flashcard_Detail",
                        principalColumn: "flashcard_detail_id");
                    table.ForeignKey(
                        name: "FK_UserFlashcardProgress_UserFlashcard",
                        column: x => x.user_flashcard_id,
                        principalTable: "User_Flashcard",
                        principalColumn: "user_flashcard_id");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    transaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_premium_package_id = table.Column<int>(type: "int", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    transaction_status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    transaction_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    payment_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_Transaction_PaymentType",
                        column: x => x.payment_type_id,
                        principalTable: "Payment_Type",
                        principalColumn: "payment_type_id");
                    table.ForeignKey(
                        name: "FK_Transaction_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_Transaction_UserPremiumPackage",
                        column: x => x.user_premium_package_id,
                        principalTable: "User_Premium_Package",
                        principalColumn: "user_premium_package_id");
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
                    partition_tag_id = table.Column<int>(type: "int", nullable: false),
                    cloud_resource_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSectionPartition", x => x.test_section_part_id);
                    table.ForeignKey(
                        name: "FK_TestSectionPartition_CloudResource",
                        column: x => x.cloud_resource_id,
                        principalTable: "Cloud_Resource",
                        principalColumn: "cloud_resource_id");
                    table.ForeignKey(
                        name: "FK_TestSectionPartition_Tag",
                        column: x => x.partition_tag_id,
                        principalTable: "Partition_Tag",
                        principalColumn: "partition_tag_id");
                    table.ForeignKey(
                        name: "FK_TestSectionPartition_TestSection",
                        column: x => x.test_section_id,
                        principalTable: "Test_Section",
                        principalColumn: "test_section_id",
                        onDelete: ReferentialAction.Cascade);
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
                    accuracy_rate = table.Column<double>(type: "float", nullable: true),
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
                        principalColumn: "test_history_id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "test_section_part_id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "partition_history_id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Speaking_Part_topic_sample_id",
                table: "Speaking_Part",
                column: "topic_sample_id");

            migrationBuilder.CreateIndex(
                name: "IX_Speaking_Topic_Sample_topic_id",
                table: "Speaking_Topic_Sample",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_test_category_id",
                table: "Test",
                column: "test_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_user_id",
                table: "Test",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Grade_partition_history_id",
                table: "Test_Grade",
                column: "partition_history_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Grade_question_id",
                table: "Test_Grade",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_History_score_calculation_id",
                table: "Test_History",
                column: "score_calculation_id");

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
                name: "IX_Test_Section_cloud_resource_id",
                table: "Test_Section",
                column: "cloud_resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Section_test_id",
                table: "Test_Section",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_Test_Section_Partition_cloud_resource_id",
                table: "Test_Section_Partition",
                column: "cloud_resource_id");

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
                name: "IX_Transaction_payment_type_id",
                table: "Transaction",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_user_id",
                table: "Transaction",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_user_premium_package_id",
                table: "Transaction",
                column: "user_premium_package_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User__EA2ECA19E6DC35F4",
                table: "User",
                column: "clerk_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Flashcard_flashcard_id",
                table: "User_Flashcard",
                column: "flashcard_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Flashcard_user_id",
                table: "User_Flashcard",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Flashcard_Progress_flashcard_detail_id",
                table: "User_Flashcard_Progress",
                column: "flashcard_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Flashcard_Progress_user_flashcard_id",
                table: "User_Flashcard_Progress",
                column: "user_flashcard_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Premium_Package_premium_package_id",
                table: "User_Premium_Package",
                column: "premium_package_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User_Pre__B9BE370EE1A5DD97",
                table: "User_Premium_Package",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Speaking_Sample_History_topic_sample_id",
                table: "User_Speaking_Sample_History",
                column: "topic_sample_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Speaking_Sample_History_user_id",
                table: "User_Speaking_Sample_History",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Question_Answer");

            migrationBuilder.DropTable(
                name: "Speaking_Part");

            migrationBuilder.DropTable(
                name: "Test_Grade");

            migrationBuilder.DropTable(
                name: "Test_Tag");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "User_Flashcard_Progress");

            migrationBuilder.DropTable(
                name: "User_Speaking_Sample_History");

            migrationBuilder.DropTable(
                name: "Partition_History");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Payment_Type");

            migrationBuilder.DropTable(
                name: "User_Premium_Package");

            migrationBuilder.DropTable(
                name: "Flashcard_Detail");

            migrationBuilder.DropTable(
                name: "User_Flashcard");

            migrationBuilder.DropTable(
                name: "Speaking_Topic_Sample");

            migrationBuilder.DropTable(
                name: "Test_History");

            migrationBuilder.DropTable(
                name: "Test_Section_Partition");

            migrationBuilder.DropTable(
                name: "Premium_Package");

            migrationBuilder.DropTable(
                name: "Flashcard");

            migrationBuilder.DropTable(
                name: "Speaking_Topic");

            migrationBuilder.DropTable(
                name: "Score_Calculation");

            migrationBuilder.DropTable(
                name: "Partition_Tag");

            migrationBuilder.DropTable(
                name: "Test_Section");

            migrationBuilder.DropTable(
                name: "Cloud_Resource");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Test_Category");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "System_Role");
        }
    }
}
