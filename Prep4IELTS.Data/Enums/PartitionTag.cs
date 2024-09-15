using System.ComponentModel;

namespace Prep4IELTS.Data.Enum;

public enum PartitionTag
{
    // LISTENING
    [Description("[Listening] Matching")]
    ListeningMatching,
    
    [Description("[Listening] Multiple Choice")]
    ListeningMultipleChoice,
    
    [Description("[Listening] Note/Form Completion")]
    ListeningNoteOrFormCompletion,
    
    [Description("[Listening] Map/Plan/Diagram Labelling")]
    ListeningMapOrDiagramLabelling,
    
    [Description("[Listening] Sentence Completion")]
    ListeningSentenceCompletion,
    
    [Description("[Listening] Short Answer")]
    ListeningShortAnswer,
    
    [Description("[Listening] Summary/Flow chart Completion")]
    ListeningSummaryOrFlowchartCompletion,
    
    [Description("[Listening] Table Completion")]
    ListeningTableCompletion,
    
    [Description("[Listening] Pick from a List")]
    ListeningPickFromList,
    
    // READING
    [Description("[Reading] True/False/Not Given")]
    ReadingTrueFalseNotGiven,
    
    [Description("[Reading] Yes/No/Not Given")]
    ReadingYesNoNotGiven,
    
    [Description("[Reading] Multiple Choice")]
    ReadingMultipleChoice,
    
    [Description("[Reading] Summary Completion")]
    ReadingSummaryCompletion,
    
    [Description("[Reading] Diagram Label Completion")]
    ReadingDiagramLabelCompletion,
    
    [Description("[Reading] Short Answer")]
    ReadingShortAnswer,
    
    [Description("[Reading] Table/Note/Flow chart Completion")]
    ReadingTableNoteOrFlowchartCompletion,
    
    [Description("[Reading] Sentencce Completion")]
    ReadingSentenceCompletion,
    
    [Description("[Reading] Matching Headings")]
    ReadingMatchingHeadings,
    
    [Description("[Reading] Matching Information to Paragraphs")]
    ReadingMatchingInformationToParagraphs,
    
    [Description("[Reading] Matching Features")]
    ReadingMatchingFeatures,
    
    [Description("[Reading] Matching Sentence Endings")]
    ReadingMatchingSentenceEndings,
    
    [Description("[Reading] Matching Name")]
    ReadingMatchingName,
    
    [Description("[Reading] Identifying Writer's Views/Claims (Agree/Disagree/Not Given)")]
    ReadingAgreeDisagreeNotGiven,

    [Description("[Reading] Choosing from a List")]
    ReadingChoosingFromList,
}