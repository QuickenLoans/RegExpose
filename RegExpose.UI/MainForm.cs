using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using RegExpose.Nodes;
using RegExpose.Nodes.Alternation;

namespace RegExpose.UI
{
    public partial class MainForm : Form
    {
        private readonly Font _inputFont;

        private readonly bool _isPresentation;
        private readonly string _commandLinePattern;
        private readonly string _commandLineInput;

        private Point _regexIndexPosition = Point.Empty;
        private Point _currentIndexPosition = Point.Empty;
        private Point _lookAroundIndexPosition = Point.Empty;
        private Point[] _savedStatesIndexPositions = new Point[0];
        private Regex _regex;

        public MainForm(bool isPresentation, string pattern, string input)
        {
            InitializeComponent();

            _isPresentation = isPresentation;
            _commandLinePattern = pattern;
            _commandLineInput = input;

            if (isPresentation)
            {
                KeyDown += ControlOnKeyDown;
                //txtInput.KeyDown += ControlOnKeyDown;
            }

            txtPattern.Text = pattern;
            txtInput.Text = input;

            lvMessages.FullRowSelect = true;
            lvMessages.HideSelection = false;

            _inputFont = txtInput.Font;

            txtInput.Paint += TxtInputOnPaint;
        }

        private void ControlOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys.LButton | Keys.RButton | Keys.Back | Keys.ShiftKey) || e.KeyCode == (Keys)116)
            {
                if (btnCompile.Enabled)
                {
                    if (btnParse.Enabled)
                    {
                        ParseInput();
                    }
                    else
                    {
                        CompileRegex();
                    }
                }
            }
            else if (e.KeyCode == (Keys.RButton | Keys.Space))
            {
                if (btnCompile.Enabled && btnParse.Enabled)
                {
                    if (lvMessages.Items.Count > 0)
                    {
                        if (lvMessages.SelectedIndices.Count == 0)
                        {
                            lvMessages.SelectedIndices.Add(0);
                        }
                        else if (lvMessages.SelectedIndices[0] < lvMessages.Items.Count - 1)
                        {
                            var currentIndex = lvMessages.SelectedIndices[0];
                            lvMessages.SelectedIndices.Clear();
                            lvMessages.SelectedIndices.Add(currentIndex + 1);
                        }
                    }
                }
                else
                {
                    splitContainer3.Panel2Collapsed = !splitContainer3.Panel2Collapsed;
                }

                e.SuppressKeyPress = _isPresentation;
            }
            else if (e.KeyCode == (Keys.LButton | Keys.Space))
            {
                if (btnCompile.Enabled && btnParse.Enabled)
                {
                    if (lvMessages.Items.Count > 0)
                    {
                        if (lvMessages.SelectedIndices.Count == 0)
                        {
                            lvMessages.SelectedIndices.Add(0);
                        }
                        else if (lvMessages.SelectedIndices[0] > 0)
                        {
                            var currentIndex = lvMessages.SelectedIndices[0];
                            lvMessages.SelectedIndices.Clear();
                            lvMessages.SelectedIndices.Add(currentIndex - 1);
                        }
                    }
                }
                else
                {
                    splitContainer3.Panel1Collapsed = !splitContainer3.Panel1Collapsed;
                }

                e.SuppressKeyPress = _isPresentation;
            }
            else if (e.KeyCode == (Keys.RButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17))
            {
                Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_commandLinePattern != null)
            {
                txtPattern.Text = _commandLinePattern;
            }

            if (_commandLineInput != null)
            {
                txtInput.Text = _commandLineInput;
            }
        }

        private void LvMessagesOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMessages.SelectedIndices.Count == 0)
            {
                return;
            }

            var cachedStep = lvMessages.Items[lvMessages.SelectedIndices[0]].Tag as CachedStep;

            if (cachedStep == null)
            {
                return;
            }

            OnStepChanged(cachedStep);
        }

        private void OnStepChanged(CachedStep cachedStep)
        {
            var step = cachedStep.Step;

            if (step.Type != ParseStepType.Error)
            {
                using (this.LockWindowUpdate())
                {
                    txtPattern.TextChanged -= TxtPatternOnTextChanged;
                    txtInput.TextChanged -= TxtInputOnTextChanged;

                    lblMessage.Text =
                        step.Type == ParseStepType.Pass || step.Type == ParseStepType.Fail || step.Type == ParseStepType.Match
                        ? step.Message
                        : string.Format("{0} - {1} /{2}/", step.Message, step.NodeType, step.Pattern);

                    txtPattern.ClearHighlights();
                    txtInput.ClearHighlights();

                    ClearTreeNodeSelections();
                    var foundNode = FindNodeById(tvRegex.Nodes, step.Node.Id.ToString(CultureInfo.InvariantCulture));
                    if (foundNode != null)
                    {
                        SetNodeBackColor(foundNode, Color.Yellow);
                    }

                    txtPattern.SetHighlight(step.Node.Index, step.Node.Pattern.Length, Color.Yellow);

                    lvMessages.SelectedIndexChanged -= LvMessagesOnSelectedIndexChanged;
                    foreach (ListViewItem item in lvMessages.Items)
                    {
                        var itemCachedStep = (CachedStep)item.Tag;
                        if (itemCachedStep.StepIndex == cachedStep.StepIndex)
                        {
                            item.Selected = true;
                            lvMessages.Select();
                            item.EnsureVisible();
                        }
                    }
                    lvMessages.SelectedIndexChanged += LvMessagesOnSelectedIndexChanged;

                    var regex = step.Node as Regex;
                    if (regex != null && step.Type == ParseStepType.Match)
                    {
                        txtInput.SetHighlight(step.InitialState.Index, step.MatchedText.Length, Color.LawnGreen);
                    }
                    else
                    {
                        if (step.Type == ParseStepType.Pass || step.Type == ParseStepType.Capture)
                        {
                            txtInput.SetHighlight(step.InitialState.Index, step.MatchedText.Length, Color.LightSkyBlue);
                        }
                        else if (step.Type == ParseStepType.Fail)
                        {
                            txtInput.SetHighlight(step.InitialState.Index, (step.CurrentState.Index - step.InitialState.Index) + 1, Color.PeachPuff);
                        }
                    }

                    _regexIndexPosition = txtInput.GetPositionFromCharIndex(cachedStep.StepIndex == 0 ? 0 : cachedStep.RegexIndex);
                    _currentIndexPosition = txtInput.GetPositionFromCharIndex(cachedStep.StepIndex == 0 ? 0 : cachedStep.CurrentIndex);

                    if (cachedStep.CurrentLookaroundIndex == -1)
                    {
                        _lookAroundIndexPosition = Point.Empty;
                    }
                    else
                    {
                        _lookAroundIndexPosition = txtInput.GetPositionFromCharIndex(cachedStep.StepIndex == 0 ? 0 : cachedStep.CurrentLookaroundIndex);
                    }

                    _savedStatesIndexPositions = cachedStep.SavedStatesIndexes.Select(index => txtInput.GetPositionFromCharIndex(index)).ToArray();
                    txtInput.Invalidate();

                    txtPattern.TextChanged += TxtPatternOnTextChanged;
                    txtInput.TextChanged += TxtInputOnTextChanged;
                }
            }
            else
            {
                MessageBox.Show(step.Exception.ToString());
            }
        }

        private void TxtPatternOnTextChanged(object sender, EventArgs e)
        {
            _regex = null;
            tvRegex.Nodes.Clear();
            btnParse.Enabled = false;

            ClearInput();
        }

        private void TxtInputOnTextChanged(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void ClearInput()
        {
            txtPattern.TextChanged -= TxtPatternOnTextChanged;
            txtInput.TextChanged -= TxtInputOnTextChanged;

            txtPattern.ClearHighlights();
            txtInput.ClearHighlights();

            _regexIndexPosition = Point.Empty;
            _currentIndexPosition = Point.Empty;
            _lookAroundIndexPosition = Point.Empty;
            _savedStatesIndexPositions = new Point[0];
            txtInput.Invalidate();

            lblMessage.Clear();
            lvMessages.Items.Clear();

            txtPattern.TextChanged += TxtPatternOnTextChanged;
            txtInput.TextChanged += TxtInputOnTextChanged;
        }

        private void TxtInputOnPaint(object sender, PaintEventArgs e)
        {
            var size = e.Graphics.MeasureString("x", _inputFont);

            if (_savedStatesIndexPositions.Any())
            {
                var savedStateColor = Color.Blue;
                var list = new List<Tuple<Point, Color>>();
                foreach (var indexPosition in _savedStatesIndexPositions)
                {
                    list.Add(Tuple.Create(indexPosition, savedStateColor));
                    savedStateColor = Color.FromArgb(Math.Max(savedStateColor.A - 25, 0),
                                                     savedStateColor.R,
                                                     savedStateColor.G,
                                                     savedStateColor.B);
                }

                foreach (var t in list)
                {
                    var p = new PointF(t.Item1.X, t.Item1.Y + (size.Height * 0.9f));
                    e.Graphics.DrawLine(new Pen(t.Item2, 3), p.X + 1, p.Y - 1, p.X - 10, p.Y + 10);
                    e.Graphics.DrawLine(new Pen(t.Item2, 3), p.X - 1, p.Y - 1, p.X + 10, p.Y + 10);
                }
            }

            if (_regexIndexPosition != Point.Empty)
            {
                var p = new PointF(_regexIndexPosition.X, _regexIndexPosition.Y + (size.Height * 0.8f));

                e.Graphics.DrawLine(new Pen(Color.Black, 5), p.X + 2, p.Y - 2, p.X - 10.5f, p.Y + 10.5f);
                e.Graphics.DrawLine(new Pen(Color.Black, 5), p.X - 2, p.Y - 2, p.X + 10f, p.Y + 10f);
            }

            if (_currentIndexPosition != Point.Empty)
            {
                var p = new PointF(_currentIndexPosition.X, _currentIndexPosition.Y + (size.Height * 0.8f));

                e.Graphics.DrawLine(new Pen(Color.Red, 3), p.X + 1, p.Y - 1, p.X - 10, p.Y + 10);
                e.Graphics.DrawLine(new Pen(Color.Red, 3), p.X - 1, p.Y - 1, p.X + 10, p.Y + 10);
            }

            if (_lookAroundIndexPosition != Point.Empty)
            {
                var p = new PointF(_lookAroundIndexPosition.X, _lookAroundIndexPosition.Y + (size.Height * 0.7f));

                e.Graphics.DrawLine(new Pen(Color.MediumPurple, 3), p.X + 1, p.Y - 1, p.X - 10, p.Y + 10);
                e.Graphics.DrawLine(new Pen(Color.MediumPurple, 3), p.X - 1, p.Y - 1, p.X + 10, p.Y + 10);
            }
        }

        private void BtnCompileOnClick(object sender, EventArgs e)
        {
            CompileRegex();
        }

        private void BtnParseOnClick(object sender, EventArgs e)
        {
            ParseInput();
        }

        private void CompileRegex()
        {
            try
            {
                if (string.IsNullOrEmpty(txtPattern.Text))
                {
                    MessageBox.Show("No pattern provided!");
                    _regex = null;
                    return;
                }

                using (this.LockWindowUpdate())
                {
                    var compiler = new RegexCompiler(ignoreCaseToolStripMenuItem.Checked,
                                                     singleLineToolStripMenuItem.Checked,
                                                     multiLineToolStripMenuItem.Checked);

                    try
                    {
                        _regex = compiler.Compile(txtPattern.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        _regex = null;
                        return;
                    }

                    tvRegex.Nodes.Clear();
                    AddNodes(tvRegex.Nodes, _regex);
                    tvRegex.ExpandAll();
                }
            }
            finally
            {
                btnParse.Enabled = _regex != null;
            }
        }

        private void ParseInput()
        {
            lvMessages.Items.Clear();

            IEnumerable<CachedStep> cachedSteps;

            try
            {
                var engine = _regex.Parse(txtInput.Text);
                cachedSteps = LoadSteps(engine.GetParseSteps()).ToList(); // Calling ToList() will force any exception to be thrown right here.
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(@"Error during parse operation:{0}{1}", Environment.NewLine, ex.Message));
                return;
            }

            foreach (var cachedStep in cachedSteps)
            {
                var item = new ListViewItem("") { UseItemStyleForSubItems = false };

                if (cachedStep.Step.Type != ParseStepType.Error)
                {
                    item.SubItems.Add(cachedStep.StepIndex.ToString(CultureInfo.InvariantCulture))
                        .BackColor = cachedStep.Step.Type == ParseStepType.Match ? Color.LawnGreen : item.BackColor;

                    item.SubItems.Add(cachedStep.Step.Type.ToString()).BackColor =
                        cachedStep.Step.Type == ParseStepType.Match
                            ? Color.LawnGreen
                            : item.BackColor;

                    item.SubItems.Add(cachedStep.Step.NodeType).BackColor =
                        cachedStep.Step.Type == ParseStepType.Match
                            ? Color.LawnGreen
                            : item.BackColor;

                    item.SubItems.Add(cachedStep.Step.Pattern).BackColor =
                        cachedStep.Step.Type == ParseStepType.Match
                            ? Color.LawnGreen
                            : item.BackColor;

                    item.SubItems.Add(cachedStep.Step.Message).BackColor =
                        cachedStep.Step.Type == ParseStepType.Match
                            ? Color.LawnGreen
                            : item.BackColor;
                }
                else
                {
                    item.UseItemStyleForSubItems = false;

                    item.SubItems.Add(cachedStep.StepIndex.ToString(CultureInfo.InvariantCulture), Color.White, Color.Red, item.Font);
                    item.SubItems.Add("???", Color.White, Color.Red, item.Font);
                    item.SubItems.Add("???", Color.White, Color.Red, item.Font);
                    item.SubItems.Add("???", Color.White, Color.Red, item.Font);
                    item.SubItems.Add(cachedStep.Step.Exception.Message, Color.White, Color.Red, item.Font);
                }

                item.Tag = cachedStep;
                lvMessages.Items.Add(item);
            }

            columnHeaderIndex.Width = -1;
            columnHeaderType.Width = -1;
            columnHeaderNodeType.Width = -1;
            columnHeaderPattern.Width = -1;
            columnHeaderMessage.Width = -1;
        }

        private IEnumerable<CachedStep> LoadSteps(IEnumerable<ParseStep> steps)
        {
            var savedStates = new Stack<int>();
            var currentIndex = 0;
            var currentLookaroundIndex = -1;
            var stepIndex = 0;
            var regexIndex = 0;

            foreach (var step in steps)
            {
                switch (step.Type)
                {
                    case ParseStepType.BeginParse:
                        if (step.Node is Regex)
                        {
                            regexIndex = step.InitialState.Index;
                        }
                        break;
                    case ParseStepType.StateSaved:
                        savedStates.Push(step.CurrentState.Index);
                        break;
                    case ParseStepType.Match:
                        savedStates.Clear();
                        currentLookaroundIndex = -1;
                        break;
                    case ParseStepType.ResetIndex:
                        currentIndex = step.InitialState.Index;
                        currentLookaroundIndex = -1;
                        break;
                    case ParseStepType.AdvanceIndex:
                        currentIndex = step.CurrentState.Index;
                        currentLookaroundIndex = -1;

                        if (step.Node is Regex)
                        {
                            regexIndex = step.CurrentState.Index;
                        }
                        break;
                    case ParseStepType.Backtrack:
                        currentIndex = savedStates.Pop();
                        break;
                    case ParseStepType.LookaroundStart:
                    case ParseStepType.LookaroundResetIndex:
                    case ParseStepType.LookaroundAdvanceIndex:
                        currentLookaroundIndex = step.CurrentState.Index;
                        break;
                    case ParseStepType.LookaroundEnd:
                        currentLookaroundIndex = -1;
                        break;
                }
                
                yield return new CachedStep(step, stepIndex, regexIndex, currentIndex, currentLookaroundIndex, savedStates.ToArray());
                stepIndex++;
            }
        }

        private static void AddNodes(TreeNodeCollection treeNodes, RegexNode regexNode)
        {
            var treeNode = treeNodes.Add(
                regexNode.Id.ToString(CultureInfo.InvariantCulture),
                string.Format("{0} - {1}", regexNode.Pattern, regexNode.NodeType));

            if (regexNode is LeafNode)
            {
                return;
            }

            var wrapperNode = regexNode as WrapperNode;
            if (wrapperNode != null)
            {
                AddNodes(treeNode.Nodes, (wrapperNode).Child);
            }

            var containerNode = regexNode as ContainerNode;
            if (containerNode != null)
            {
                foreach (var child in (containerNode).Children)
                {
                    AddNodes(treeNode.Nodes, child);
                }
            }

            var alternation = regexNode as Alternation;
            if (alternation != null)
            {
                foreach (var choice in (alternation).Choices)
                {
                    AddNodes(treeNode.Nodes, choice);
                }
            }
        }

        private void TvRegexOnBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // Don't allow any node to be selected (selected nodes are blue and ugly)
            e.Cancel = true;
        }

        private void ClearTreeNodeSelections()
        {
            foreach (TreeNode node in tvRegex.Nodes)
            {
                SetNodeBackColor(node, tvRegex.BackColor);
            }
        }

        private static void SetNodeBackColor(TreeNode node, Color color)
        {
            node.BackColor = color;

            foreach (TreeNode child in node.Nodes)
            {
                SetNodeBackColor(child, color);
            }
        }

        private static TreeNode FindNodeById(TreeNodeCollection nodes, string id)
        {
            var foundNode = nodes[id];
            
            if (foundNode != null)
            {
                return foundNode;
            }

            foreach (TreeNode child in nodes)
            {
                foundNode = FindNodeById(child.Nodes, id);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }
    }
}
