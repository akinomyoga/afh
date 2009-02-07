using Gen=System.Collections.Generic;

namespace afh.File.Riff{
	public class RiffWriter{
		RiffFile file;
		Gen::List<System.IO.Stream> streams=new Gen::List<System.IO.Stream>();

		bool written=false;
		const string ERR_WRITTEN="既に RiffWriter は書込を完了しました。追加の書込を行う事は出来ません。";

		public RiffWriter(string filetype){
			this.file=new RiffFile(filetype);
		}
		public void AddChunk(string name,object content){
			if(written)throw new System.ObjectDisposedException("this",ERR_WRITTEN);

			Chunk chunk=new Chunk(name);
			chunk.SetContent(content);
			this.file.Chunks.Add(chunk);
		}
		public void AddChunk(string name,out StreamAccessor ac_chunk){
			if(written)throw new System.ObjectDisposedException("this",ERR_WRITTEN);

			Chunk chunk=new Chunk(name);
			this.file.Chunks.Add(chunk);

			System.IO.MemoryStream str=new System.IO.MemoryStream();
			chunk.Stream=str;
			this.streams.Add(str);

			ac_chunk=new StreamAccessor(str);
		}
		public void Write(StreamAccessor accessor){
			this.written=true;

			accessor.WriteAs<RiffFile>(this.file);

			foreach(System.IO.Stream stream in this.streams)stream.Close();
		}
	}
}