using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace MyKTV_Management_Studio
{
    public partial class FrmSongsInfo : OfficeForm
    {
        public FrmSongsInfo()
        {
            InitializeComponent();
            this.EnableGlass = false;
        }

        ISongService songservice = new SongServiceImpl();
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
        private List<Song> songlist;
        private List<Song> songfilteredlist;
        private Dictionary<string, MyInfo> singers;
        private List<string> singernamelist;
        private Pagination<Song> paginations;
        private bool issonglistpagingshow;
        private bool issongfilteredlistpagingshow;
        private int pagesize = 10;
        private int currentpageindex;
        private int lastpageindex;
        private int pagecount;
        private int currentfirstindex;
        private int currentlastindex;
        private string type;
        private string pinyin;
        private int wordcount;
        private string singername;

        private void FrmSongsInfo_Load(object sender, EventArgs e)
        {
            issonglistpagingshow = false;
            singers = InfoUtil.GetInfoByFile(PARAMS.BINFilePath[1]);
            songlist = songservice.GetSongs(SongsListType.SongsList);
            BindDataSource();
            WriteBinaryDataToFile();
            BindAutoCompleteSourceData();
            cbSongType.ValueMember = "Key";
            cbSongType.DisplayMember = "Value";
            InitControls();
            PARAMS.SongListStatus = 0;
            PARAMS.HasSongsFiltered = false;
        }

        private void BindDataSource()
        {
            BindingSource songtypesource = new BindingSource
            {
                DataSource = songservice.GetSongTypes()
            };
            this.cbSongType.DataSource = songtypesource;
            BindingSource sorttypesource = new BindingSource
            {
                DataSource = songservice.GetAllSortTypes()
            };
            this.cbSortType.DataSource = sorttypesource;
        }

        private void InitControls()
        {

            if (issonglistpagingshow)
            {
                this.bar3.Visible = true;
                currentpageindex = 1;
                this.cbSongType.SelectedIndex = 0;
                this.inttxtWordCount.Value = 0;
                this.inttxtWordCount.MaxValue = songservice.GetMaxSongWordCount();
                this.chbSort.Checked = false;
                this.cbSortType.Enabled = false;
                ClearDGVData();
                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                lastpageindex = paginations.PageCount;
                this.dgvSongsInfo.DataSource = paginations;
                this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                this.ttxtPagePosition.Text = currentpageindex.ToString();
                this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                pagecount = paginations.PageCount;
                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                this.tlblRecordCount.Text = songlist.Count.ToString();
                currentfirstindex = 1;
                currentlastindex = pagesize;
                if (!paginations.HasNextPage)
                {
                    this.tbiMoveNextPage.Enabled = false;
                    this.tbiMoveLastPage.Enabled = false;
                    currentlastindex = songlist.Count;
                    this.tbiNavigation.Enabled = false;
                }
                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                this.tbiMovePreviousPage.Enabled = false;
                this.tbiMoveFirstPage.Enabled = false;
                if (this.dgvSongsInfo.Rows.Count == 0)
                {
                    this.bar3.Visible = false;
                }
            }
            else if (!issonglistpagingshow)
            {
                this.bar3.Visible = false;
                this.cbSongType.SelectedIndex = 0;
                this.inttxtWordCount.Value = 0;
                this.inttxtWordCount.MaxValue = songservice.GetMaxSongWordCount();
                this.chbSort.Checked = false;
                this.cbSortType.Enabled = false;
                ClearDGVData();
                this.dgvSongsInfo.DataSource = songlist;
            }
        }

        private void ClearDGVData()
        {
            if (this.dgvSongsInfo.Rows.Count != 0)
            {
                IList<Song> list = (IList<Song>)this.dgvSongsInfo.DataSource;
                for (int i = 0; i < list.Count; i++)
                {
                    list.RemoveAt(i);
                }
                this.dgvSongsInfo.DataSource = null;
            }
        }

        private void dgvSongsInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < this.dgvSongsInfo.Rows.Count; i++)
            {
                this.dgvSongsInfo.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void tbiMoveFirstPage_Click(object sender, EventArgs e)
        {
            currentpageindex = 1;
            this.tbiMoveFirstPage.Enabled = false;
            this.tbiMovePreviousPage.Enabled = false;
            this.tbiMoveNextPage.Enabled = true;
            this.tbiMoveLastPage.Enabled = true;
            type = this.cbSongType.Text;
            pinyin = this.txtPinYin.Text.Trim().ToUpper();
            wordcount = this.inttxtWordCount.Value;
            singername = this.txtSinger.Text.Trim();
            songfilteredlist = new List<Song>();
            if (!PARAMS.HasSongsFiltered)
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 5:
                        songlist = songservice.GetSongs(SongsListType.SongsAscList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
            else
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
        }
        private void tbiMovePreviousPage_Click(object sender, EventArgs e)
        {
            this.tbiMoveNextPage.Enabled = true;
            this.tbiMoveLastPage.Enabled = true;
            type = this.cbSongType.Text;
            pinyin = this.txtPinYin.Text.Trim().ToUpper();
            wordcount = this.inttxtWordCount.Value;
            singername = this.txtSinger.Text.Trim();
            songfilteredlist = new List<Song>();
            if (!PARAMS.HasSongsFiltered)
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 5:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
            else
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        currentpageindex -= 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                        currentfirstindex -= pagesize;
                        if (paginations.HasNextPage || paginations.HasPreviousPage)
                        {
                            currentlastindex = currentpageindex * pagesize;
                        }
                        if (!paginations.HasPreviousPage)
                        {
                            this.tbiMoveFirstPage.Enabled = false;
                            this.tbiMovePreviousPage.Enabled = false;
                            currentfirstindex = 1;
                            currentlastindex = pagesize;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
        }

        private void tbiMoveNextPage_Click(object sender, EventArgs e)
        {
            this.tbiMoveFirstPage.Enabled = true;
            this.tbiMovePreviousPage.Enabled = true;
            type = this.cbSongType.Text;
            pinyin = this.txtPinYin.Text.Trim().ToUpper();
            wordcount = this.inttxtWordCount.Value;
            singername = this.txtSinger.Text.Trim();
            songfilteredlist = new List<Song>();
            if (!PARAMS.HasSongsFiltered)
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 5:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscList);
                        paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
            else
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songfilteredlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songfilteredlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songfilteredlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songfilteredlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        currentpageindex += 1;
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                        }
                        currentfirstindex += pagesize;
                        currentlastindex += pagesize;
                        if (!paginations.HasNextPage)
                        {
                            currentlastindex = songfilteredlist.Count;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
        }

        private void tbiMoveLastPage_Click(object sender, EventArgs e)
        {
            this.tbiMoveFirstPage.Enabled = true;
            this.tbiMovePreviousPage.Enabled = true;
            this.tbiMoveNextPage.Enabled = false;
            this.tbiMoveLastPage.Enabled = false;
            type = this.cbSongType.Text;
            pinyin = this.txtPinYin.Text.Trim().ToUpper();
            wordcount = this.inttxtWordCount.Value;
            singername = this.txtSinger.Text.Trim();
            songfilteredlist = new List<Song>();
            if (!PARAMS.HasSongsFiltered)
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        paginations = new Pagination<Song>(songlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        paginations = new Pagination<Song>(songlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 5:
                        songlist = songservice.GetSongs(SongsListType.SongsAscList);
                        paginations = new Pagination<Song>(songlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
            else
            {
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songfilteredlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songfilteredlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songfilteredlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songfilteredlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, lastpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        currentfirstindex = pagesize * (pagecount - 1) + 1;
                        currentlastindex = songfilteredlist.Count;
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            PARAMS.HasSongsFiltered = true;
            this.bar1.Enabled = false;
            this.tbiMoveFirstPage.Enabled = false;
            this.tbiMovePreviousPage.Enabled = false;
            this.tbiMoveNextPage.Enabled = true;
            this.tbiMoveLastPage.Enabled = true;
            this.tbiNavigation.Enabled = true;
            type = this.cbSongType.Text;
            pinyin = this.txtPinYin.Text.Trim().ToUpper();
            wordcount = this.inttxtWordCount.Value;
            singername = this.txtSinger.Text.Trim();
            songfilteredlist = new List<Song>();
            if (type == "全部" && string.IsNullOrEmpty(pinyin) && wordcount == 0 && string.IsNullOrEmpty(singername) && !this.chbSort.Checked && !this.chbPagingShow.Checked)
            {
                PARAMS.HasSongsFiltered = false;
            }
            if (issongfilteredlistpagingshow)
            {
                currentpageindex = 1;
                this.ttxtRecordCountPerPage.Text = pagesize.ToString();
                this.bar3.Visible = true;
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        lastpageindex = pagecount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                            currentlastindex = songfilteredlist.Count;
                            this.tbiNavigation.Enabled = false;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 1:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        lastpageindex = pagecount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                            currentlastindex = songfilteredlist.Count;
                            this.tbiNavigation.Enabled = false;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 2:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        lastpageindex = pagecount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                            currentlastindex = songfilteredlist.Count;
                            this.tbiNavigation.Enabled = false;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 3:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        lastpageindex = pagecount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                            currentlastindex = songfilteredlist.Count;
                            this.tbiNavigation.Enabled = false;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                    case 4:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = paginations;
                        pagecount = paginations.PageCount;
                        lastpageindex = pagecount;
                        currentfirstindex = 1;
                        currentlastindex = pagesize;
                        if (!paginations.HasNextPage)
                        {
                            this.tbiMoveNextPage.Enabled = false;
                            this.tbiMoveLastPage.Enabled = false;
                            currentlastindex = songfilteredlist.Count;
                            this.tbiNavigation.Enabled = false;
                        }
                        this.ttxtPagePosition.Text = currentpageindex.ToString();
                        this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                        this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                        this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                        break;
                }
            }
            else
            {
                this.bar3.Visible = false;
                switch (PARAMS.SongListStatus)
                {
                    case 0:
                        songlist = songservice.GetSongs(SongsListType.SongsList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = songfilteredlist;
                        break;
                    case 1:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = songfilteredlist;
                        break;
                    case 2:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = songfilteredlist;
                        break;
                    case 3:
                        songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = songfilteredlist;
                        break;
                    case 4:
                        songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                        songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                        ClearDGVData();
                        this.dgvSongsInfo.DataSource = songfilteredlist;
                        break;
                }
            }
            if (this.dgvSongsInfo.Rows.Count == 0)
            {
                this.bar3.Visible = false;
            }
            acsc.Add(this.txtSinger.Text);
            this.txtSinger.AutoCompleteCustomSource = acsc;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.bar1.Enabled = true;
            this.tbiMoveNextPage.Enabled = true;
            this.tbiMoveLastPage.Enabled = true;
            this.tbiNavigation.Enabled = true;
            this.cbSongType.SelectedIndex = 0;
            this.txtPinYin.Text = string.Empty;
            this.inttxtWordCount.Value = 0;
            this.txtSinger.Text = string.Empty;
            songlist = songservice.GetSongs(SongsListType.SongsList);
            InitControls();
            PARAMS.SongListStatus = 0;
            PARAMS.HasSongsFiltered = false;
            acsc.Add(this.txtSinger.Text);
            this.txtSinger.AutoCompleteCustomSource = acsc;
            this.chbSort.Checked = false;
            this.chbPagingShow.Checked = false;
        }

        private string GetRecordIndexRange(Pagination<Song> paginations, int firstindex, int lastindex)
        {
            return firstindex != lastindex ? string.Format("{0}-{1}", firstindex, lastindex) : string.Format("{0}", firstindex);
        }

        private void ttxtRecordCountPerPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ValidationUtil.IsLegalInput(this.ttxtRecordCountPerPage.Text.Trim()))
                {
                    pagesize = Convert.ToInt32(this.ttxtRecordCountPerPage.Text.Trim());
                    if (pagesize >= PARAMS.PAGESIZE_MIN && pagesize <= PARAMS.PAGESIZE_MAX)
                    {
                        this.Focus();
                        this.tbiMoveFirstPage.Enabled = false;
                        this.tbiMovePreviousPage.Enabled = false;
                        this.tbiMoveNextPage.Enabled = true;
                        this.tbiMoveLastPage.Enabled = true;
                        type = this.cbSongType.Text;
                        pinyin = this.txtPinYin.Text.Trim().ToUpper();
                        wordcount = this.inttxtWordCount.Value;
                        singername = this.txtSinger.Text.Trim();
                        songfilteredlist = new List<Song>();
                        if (!PARAMS.HasSongsFiltered)
                        {
                            switch (PARAMS.SongListStatus)
                            {
                                case 0:
                                    songlist = songservice.GetSongs(SongsListType.SongsList);
                                    paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 1:
                                    songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                                    paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 2:
                                    songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                                    paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 3:
                                    songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                                    paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 4:
                                    songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                                    paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 5:
                                    songlist = songservice.GetSongs(SongsListType.SongsAscList);
                                    paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                            }
                        }
                        else
                        {
                            switch (PARAMS.SongListStatus)
                            {
                                case 0:
                                    songlist = songservice.GetSongs(SongsListType.SongsList);
                                    songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                    paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songfilteredlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 1:
                                    songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                                    songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                    paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songfilteredlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 2:
                                    songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                                    songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                    paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songfilteredlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 3:
                                    songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                                    songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                    paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songfilteredlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                                case 4:
                                    songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                                    songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                    paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                    ClearDGVData();
                                    this.dgvSongsInfo.DataSource = paginations;
                                    pagecount = paginations.PageCount;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                    if (!paginations.HasNextPage)
                                    {
                                        this.tbiMoveNextPage.Enabled = false;
                                        this.tbiMoveLastPage.Enabled = false;
                                        currentlastindex = songfilteredlist.Count;
                                        this.tbiNavigation.Enabled = false;
                                    }
                                    this.ttxtPagePosition.Text = currentpageindex.ToString();
                                    this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                    this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                    this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxEx.EnableGlass = false;
                        MessageBoxEx.Show(PARAMS.ERRORMSG_INPUT2, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.ttxtRecordCountPerPage.SelectAll();
                        this.ttxtRecordCountPerPage.SelectedText = string.Empty;
                    }
                }
                else
                {
                    MessageBoxEx.EnableGlass = false;
                    MessageBoxEx.Show(PARAMS.ERRORMSG_INPUT2, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.ttxtRecordCountPerPage.SelectAll();
                    this.ttxtRecordCountPerPage.SelectedText = string.Empty;
                }
            }
        }

        private void tbiNavigation_Click(object sender, EventArgs e)
        {
            if (ValidationUtil.IsLegalInput(this.ttxtPagePosition.Text.Trim()))
            {
                currentpageindex = Convert.ToInt32(this.ttxtPagePosition.Text.Trim());
                if (currentpageindex <= pagecount)
                {
                    this.tbiMoveFirstPage.Enabled = true;
                    this.tbiMovePreviousPage.Enabled = true;
                    this.tbiMoveNextPage.Enabled = true;
                    this.tbiMoveLastPage.Enabled = true;
                    type = this.cbSongType.Text;
                    pinyin = this.txtPinYin.Text.Trim().ToUpper();
                    wordcount = this.inttxtWordCount.Value;
                    singername = this.txtSinger.Text.Trim();
                    songfilteredlist = new List<Song>();
                    if (!PARAMS.HasSongsFiltered)
                    {
                        switch (PARAMS.SongListStatus)
                        {
                            case 0:
                                songlist = songservice.GetSongs(SongsListType.SongsList);
                                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                if (pagecount == 1)
                                {
                                    this.tbiNavigation.Enabled = false;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 1:
                                songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 2:
                                songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 3:
                                songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 4:
                                songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.ttxtPagePosition.Text = currentpageindex.ToString();
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 5:
                                songlist = songservice.GetSongs(SongsListType.SongsAscList);
                                paginations = new Pagination<Song>(songlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.ttxtPagePosition.Text = currentpageindex.ToString();
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                        }
                    }
                    else
                    {
                        switch (PARAMS.SongListStatus)
                        {
                            case 0:
                                songlist = songservice.GetSongs(SongsListType.SongsList);
                                songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 1:
                                songlist = songservice.GetSongs(SongsListType.SongsAscByPlayCountList);
                                songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.ttxtPagePosition.Text = currentpageindex.ToString();
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 2:
                                songlist = songservice.GetSongs(SongsListType.SongsDescByPlayCountList);
                                songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 3:
                                songlist = songservice.GetSongs(SongsListType.SongsAscByWordCountList);
                                songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                            case 4:
                                songlist = songservice.GetSongs(SongsListType.SongsDescByWordCountList);
                                songfilteredlist = songservice.GetFilteredSongs(songlist, type, pinyin, wordcount, singername);
                                paginations = new Pagination<Song>(songfilteredlist, pagesize, currentpageindex);
                                ClearDGVData();
                                this.dgvSongsInfo.DataSource = paginations;
                                pagecount = paginations.PageCount;
                                if (!paginations.HasNextPage)
                                {
                                    this.tbiMoveNextPage.Enabled = false;
                                    this.tbiMoveLastPage.Enabled = false;
                                    currentfirstindex = pagesize * (pagecount - 1) + 1;
                                    currentlastindex = songlist.Count;
                                }
                                else if (!paginations.HasPreviousPage)
                                {
                                    this.tbiMoveFirstPage.Enabled = false;
                                    this.tbiMovePreviousPage.Enabled = false;
                                    currentfirstindex = 1;
                                    currentlastindex = pagesize;
                                }
                                this.tlblPageCount.Text = string.Format("/{0}", pagecount.ToString());
                                this.tlblRecordCount.Text = songfilteredlist.Count.ToString();
                                this.tlblRecordIndexRange.Text = GetRecordIndexRange(paginations, currentfirstindex, currentlastindex);
                                break;
                        }
                    }
                }
                else
                {
                    MessageBoxEx.EnableGlass = false;
                    MessageBoxEx.Show(PARAMS.ERRORMSG_INPUT1, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBoxEx.EnableGlass = false;
                MessageBoxEx.Show(PARAMS.ERRORMSG_INPUT1, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindAutoCompleteSourceData()
        {
            this.txtSinger.AutoCompleteMode = AutoCompleteMode.Suggest;
            foreach (string name in InfoUtil.GetInfos("singername", singers))
            {
                acsc.Add(name);
            }

            this.txtSinger.AutoCompleteCustomSource = acsc;
            this.txtSinger.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void WriteBinaryDataToFile()
        {
            singernamelist = songservice.GetSingerNames();
            string key = "singername";
            if (singers.ContainsKey(key))
            {
                singers.Remove(key);
            }
            InfoUtil.SaveInfoToFile(key, singernamelist, singers, PARAMS.BINFilePath[1]);
        }

        private void tbiGlobalSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ttxtGlobalSearch.Text))
            {
                this.ttxtGlobalSearch.Focus();
            }
            switch (PARAMS.SongListStatus)
            {
                case 0:
                    songlist = songservice.GetSongsByKeyWord(songservice.GetSongs(SongsListType.SongsList), this.ttxtGlobalSearch.Text.Trim());
                    InitControls();
                    break;
                case 5:
                    songlist = songservice.GetSongsByKeyWord(songservice.GetSongs(SongsListType.SongsAscList), this.ttxtGlobalSearch.Text.Trim());
                    InitControls();
                    break;
            }
        }

        private void tbiPagingShow_Click(object sender, EventArgs e)
        {
            if (this.tbiPagingShow.Checked)
            {
                issonglistpagingshow = false;
                if (PARAMS.SongListStatus == 0)
                {
                    songlist = songservice.GetSongs(SongsListType.SongsList);
                    InitControls();
                }
                if (PARAMS.SongListStatus == 5)
                {
                    songlist = songservice.GetSongs(SongsListType.SongsAscList);
                    InitControls();
                }
                this.tbiPagingShow.Checked = false;
            }
            else
            {
                issonglistpagingshow = true;
                if (PARAMS.SongListStatus == 0)
                {
                    songlist = songservice.GetSongs(SongsListType.SongsList);
                    InitControls();
                }
                if (PARAMS.SongListStatus == 5)
                {
                    songlist = songservice.GetSongs(SongsListType.SongsAscList);
                    InitControls();
                }
                this.tbiPagingShow.Checked = true;
            }
        }

        private void chbSort_CheckedChanged(object sender, EventArgs e)
        {
            if (this.inttxtWordCount.Value != 0)
            {
                BindingSource sorttypesource = new BindingSource
                {
                    DataSource = songservice.GetSortListByHeat()
                };
                this.cbSortType.DataSource = sorttypesource;
            }
            else
            {
                BindingSource sorttypesource = new BindingSource
                {
                    DataSource = songservice.GetAllSortTypes()
                };
                this.cbSortType.DataSource = sorttypesource;
            }
            if (!this.chbSort.Checked)
            {
                PARAMS.SongListStatus = 0;
                this.cbSortType.SelectedIndex = -1;
                this.cbSortType.Enabled = false;
            }
            else if (this.chbSort.Checked)
            {
                PARAMS.SongListStatus = 1;
                this.cbSortType.SelectedIndex = 0;
                this.cbSortType.Enabled = true;
            }
        }

        private void chbPagingShow_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbPagingShow.Checked)
            {
                issongfilteredlistpagingshow = true;
            }
            else
            {
                issongfilteredlistpagingshow = false;
            }
        }

        private void tbiClear_Click(object sender, EventArgs e)
        {
            this.ttxtGlobalSearch.Text = string.Empty;
            if (PARAMS.SongListStatus == 0)
            {
                songlist = songservice.GetSongs(SongsListType.SongsList);
                InitControls();
            }
            if (PARAMS.SongListStatus == 5)
            {
                songlist = songservice.GetSongs(SongsListType.SongsAscList);
                InitControls();
            }
        }

        private void tbiSort_Click(object sender, EventArgs e)
        {
            if (this.tbiSort.Checked)
            {
                this.tbiSort.Checked = false;
                PARAMS.SongListStatus = 0;
                songlist = songservice.GetSongs(SongsListType.SongsList);
                InitControls();
                PARAMS.HasSongsFiltered = false;
            }
            else
            {
                this.tbiSort.Checked = true;
                PARAMS.SongListStatus = 5;
                songlist = songservice.GetSongs(SongsListType.SongsAscList);
                InitControls();
                PARAMS.HasSongsFiltered = false;
            }
        }

        private void inttxtWordCount_ValueChanged(object sender, EventArgs e)
        {
            if (this.inttxtWordCount.Value != 0)
            {
                BindingSource sorttypesource = new BindingSource
                {
                    DataSource = songservice.GetSortListByHeat()
                };
                this.cbSortType.DataSource = sorttypesource;
            }
            else
            {
                BindingSource sorttypesource = new BindingSource
                {
                    DataSource = songservice.GetAllSortTypes()
                };
                this.cbSortType.DataSource = sorttypesource;
            }
        }

        private void cbSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbSortType.SelectedIndex)
            {
                case 0:
                    PARAMS.SongListStatus = 1;
                    break;
                case 1:
                    PARAMS.SongListStatus = 2;
                    break;
                case 2:
                    PARAMS.SongListStatus = 3;
                    break;
                case 3:
                    PARAMS.SongListStatus = 4;
                    break;
                default:
                    break;
            }
        }
    }
}